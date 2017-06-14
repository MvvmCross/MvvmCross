// MvxAndroidViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Views.Caching;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter
        : MvxViewPresenter, IMvxAndroidViewPresenter
    {
        protected Activity Activity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";

        protected FragmentHostRegistrationSettings _fragmentHostRegistrationSettings;
        protected Lazy<IMvxNavigationSerializer> _lazyNavigationSerializerFactory;

        protected IMvxNavigationSerializer Serializer => _lazyNavigationSerializerFactory.Value;

        private IFragmentCacheConfiguration _fragmentCacheConfiguration;
        public IFragmentCacheConfiguration FragmentCacheConfiguration => _fragmentCacheConfiguration ?? (_fragmentCacheConfiguration = BuildFragmentCacheConfiguration());

        public virtual IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
        {
            return new DefaultFragmentCacheConfiguration();
        }

        protected Dictionary<Type, Action<MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;

        protected virtual void RegisterAttributeTypes()
        {
            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxFragmentAttribute),
               (attribute, request) => ShowFragment((MvxFragmentAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxDialogAttribute),
               (attribute, request) => ShowDialogFragment((MvxDialogAttribute)attribute, request));
        }

        public MvxAndroidViewPresenter(IEnumerable<Assembly> AndroidViewAssemblies)
        {
            _lazyNavigationSerializerFactory = new Lazy<IMvxNavigationSerializer>(Mvx.Resolve<IMvxNavigationSerializer>);
            _fragmentHostRegistrationSettings = new FragmentHostRegistrationSettings(AndroidViewAssemblies);

            RegisterAttributeTypes();
        }

        protected virtual void ShowFragment(
            MvxFragmentAttribute attribute,
            MvxViewModelRequest request)
        {
            //TODO: Check if Activity host is already on screen
            //TODO: Check if Activity base extends FragmentActivity
            //TODO: Load Activity if not shown yet
            //TODO: Check if FragmentContentId can be found on Activity
            //TODO: Show fragment on Activity
        }

        protected virtual void ShowDialogFragment(
            MvxDialogAttribute attribute,
            MvxViewModelRequest request)
        {
            //TODO: Check if class implements IDialogInterface
            //TODO: Check if class is a Fragment
            //TODO: Show as Dialog or DialogFragment
        }

        public override void Show(MvxViewModelRequest request)
        {
            var isFragment = _fragmentHostRegistrationSettings.IsTypeRegisteredAsFragment(request.ViewModelType);

            var attribute = _fragmentHostRegistrationSettings.GetMvxFragmentAttributeAssociatedWithCurrentHost(request.ViewModelType);

            Action<MvxBasePresentationAttribute, MvxViewModelRequest> showAction;
            if (!_attributeTypesToShowMethodDictionary.TryGetValue(attribute.GetType(), out showAction))
                throw new KeyNotFoundException($"The type {attribute.GetType().Name} is not configured in the presenter dictionary");

            showAction.Invoke(attribute, request);


            /*
            if (_fragmentHostRegistrationSettings.IsTypeRegisteredAsFragment(request.ViewModelType))
                ShowFragment(request);
            else
                ShowActivity(request);*/
        }

        protected virtual void ShowActivity(MvxViewModelRequest request, MvxViewModelRequest fragmentRequest = null)
        {
            if (fragmentRequest == null)
            {
                var intent = CreateIntentForRequest(request);
                ShowIntent(intent);
            }
            else
                Show(request, fragmentRequest);
        }

        public void Show(MvxViewModelRequest request, MvxViewModelRequest fragmentRequest)
        {
            var intent = CreateIntentForRequest(request);
            if (fragmentRequest != null)
            {
                var converter = Mvx.Resolve<IMvxNavigationSerializer>();
                var requestText = converter.Serializer.SerializeObject(fragmentRequest);
                intent.PutExtra(ViewModelRequestBundleKey, requestText);
            }

            ShowIntent(intent);
        }

        protected virtual void ShowIntent(Intent intent)
        {
            var activity = Activity;
            if (activity == null)
            {
                MvxTrace.Warning("Cannot Resolve current top activity");
                return;
            }
            activity.StartActivity(intent);
        }

        protected virtual string GetFragmentTag(MvxViewModelRequest request)
        {
            // THAT won't work properly if you have multiple instance of same fragment type in same FragmentHost.
            // Override that in such cases
            return request.ViewModelType.FullName;
        }

        protected virtual void ShowFragment(MvxViewModelRequest request)
        {
            var bundle = new Bundle();
            var serializedRequest = Serializer.Serializer.SerializeObject(request);
            bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

            if (request is MvxViewModelInstanceRequest)
            {
                Mvx.Resolve<IMvxChildViewModelCache>().Cache(((MvxViewModelInstanceRequest)request).ViewModelInstance);
            }

            if (!_fragmentHostRegistrationSettings.IsActualHostValid(request.ViewModelType))
            {
                Type newFragmentHostViewModelType =
                    _fragmentHostRegistrationSettings.GetFragmentHostViewModelType(request.ViewModelType);

                var fragmentHostMvxViewModelRequest = MvxViewModelRequest.GetDefaultRequest(newFragmentHostViewModelType);
                ShowActivity(fragmentHostMvxViewModelRequest, request);
                return;
            }

            var mvxFragmentAttributeAssociated = _fragmentHostRegistrationSettings.GetMvxFragmentAttributeAssociatedWithCurrentHost(request.ViewModelType);
            var fragmentType = _fragmentHostRegistrationSettings.GetFragmentTypeAssociatedWith(request.ViewModelType);

            var fragmentTag = GetFragmentTag(request);
            FragmentCacheConfiguration.RegisterFragmentToCache(fragmentTag, fragmentType, request.ViewModelType, mvxFragmentAttributeAssociated.AddToBackStack);

            var fragment = CreateFragment(fragmentType, bundle);

            var childViewModelCache = Mvx.GetSingleton<IMvxChildViewModelCache>();
            var viewModelType = request.ViewModelType;
            if (childViewModelCache.Exists(viewModelType))
            {
                fragment.ViewModel = childViewModelCache.Get(viewModelType);
                childViewModelCache.Remove(viewModelType);
            }

            ReplaceFragment(mvxFragmentAttributeAssociated, fragment, fragmentTag);
        }

        protected virtual void ReplaceFragment(MvxFragmentAttribute mvxFragmentAttributeAssociated, IMvxFragmentView fragment, string fragmentTag)
        {
            var ft = Activity.FragmentManager.BeginTransaction();
            ft.Replace(mvxFragmentAttributeAssociated.FragmentContentId, fragment as Fragment, fragmentTag);
        }

        protected virtual IMvxFragmentView CreateFragment(Type fragType, Bundle bundle)
        {
            return Fragment.Instantiate(Activity, FragmentJavaName(fragType),
                    bundle) as IMvxFragmentView;
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            return Class.FromType(fragmentType).Name;
        }

        protected virtual Intent CreateIntentForRequest(MvxViewModelRequest request)
        {
            IMvxAndroidViewModelRequestTranslator requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

            if (request is MvxViewModelInstanceRequest)
            {
                var instanceRequest = requestTranslator.GetIntentWithKeyFor(((MvxViewModelInstanceRequest)request).ViewModelInstance);
                return instanceRequest.Item1;
            }
            return requestTranslator.GetIntentFor(request);
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            var presentationHint = hint as MvxClosePresentationHint;
            if (presentationHint != null)
            {
                Close(presentationHint.ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            //TODO: Check if viewModel is Fragment, Dialog or Activity

            var activity = Activity;

            var currentView = activity as IMvxView;

            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return;
            }

            if (currentView.ViewModel != viewModel)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return;
            }

            activity.Finish();
        }
    }
}