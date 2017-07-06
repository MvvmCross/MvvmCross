using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Views
{
    public class MvxAndroidPresenter : MvxViewPresenter, IMvxAndroidViewPresenter
    {
        protected virtual Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        protected IEnumerable<Assembly> _androidViewAssemblies;
        protected Dictionary<Type, Action<MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;
        protected readonly Dictionary<Type, IList<MvxBasePresentationAttribute>> _fragmentTypeToPresentationAttributeMap;
        protected Dictionary<Type, Type> _viewModelToFragmentTypeMap;
        protected readonly IMvxViewModelTypeFinder _viewModelTypeFinder;

        protected Lazy<IMvxNavigationSerializer> _lazyNavigationSerializerFactory;
        protected IMvxNavigationSerializer Serializer;
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";

        public MvxAndroidPresenter(IEnumerable<Assembly> androidViewAssemblies)
        {
            _androidViewAssemblies = androidViewAssemblies;
            _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
            _fragmentTypeToPresentationAttributeMap = new Dictionary<Type, IList<MvxBasePresentationAttribute>>();
            _attributeTypesToShowMethodDictionary = new Dictionary<Type, Action<MvxBasePresentationAttribute, MvxViewModelRequest>>();

            //Serializer = Mvx.Resolve<IMvxNavigationSerializer>();

            init();

            RegisterAttributeTypes();
        }

        private void init()
        {
           var typesWithBasePresentationAttribute = _androidViewAssemblies
                        .SelectMany(x => x.DefinedTypes)
                        .Select(x => x.AsType())
                        .Where(x => x.HasBasePresentationAttribute())
                        .ToList();

            foreach (var typeWithAttribute in typesWithBasePresentationAttribute)
            {
                if (!_fragmentTypeToPresentationAttributeMap.ContainsKey(typeWithAttribute))
                    _fragmentTypeToPresentationAttributeMap.Add(typeWithAttribute, new List<MvxBasePresentationAttribute>());

                foreach (var attribute in typeWithAttribute.GetBasePresentationAttributes())
                    _fragmentTypeToPresentationAttributeMap[typeWithAttribute].Add(attribute);
            }

            _viewModelToFragmentTypeMap =
                typesWithBasePresentationAttribute.ToDictionary(GetAssociatedViewModelType, fragmentType => fragmentType);
        }

        private Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = _viewModelTypeFinder.FindTypeOrNull(fromFragmentType);
            return viewModelType ?? fromFragmentType.GetBasePresentationAttributes().First().ViewModelType;
        }

        protected virtual void RegisterAttributeTypes()
        {
            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxFragmentAttribute),
               (attribute, request) => ShowFragment((MvxFragmentAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxDialogAttribute),
               (attribute, request) => ShowDialogFragment((MvxDialogAttribute)attribute, request));
        }

        public override void Show(MvxViewModelRequest request)
        {
            if(_viewModelToFragmentTypeMap.ContainsKey(request.ViewModelType))
            {
                var attribute = GetAttributeForViewModel(request.ViewModelType);

                Action<MvxBasePresentationAttribute, MvxViewModelRequest> showAction;
                if (!_attributeTypesToShowMethodDictionary.TryGetValue(attribute.GetType(), out showAction))
                    throw new KeyNotFoundException($"The type {attribute.GetType().Name} is not configured in the presenter dictionary");

                showAction.Invoke(attribute, request);
                return;
            }
            else
            {
                var intent = CreateIntentForRequest(request);
                ShowIntent(intent);
            }
        }

        private MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType)
        {
            var fragmentType = _viewModelToFragmentTypeMap.GetValueOrDefault(viewModelType);
            var attribute = _fragmentTypeToPresentationAttributeMap.GetValueOrDefault(fragmentType).FirstOrDefault();
            attribute.ViewType = fragmentType;
            return attribute;
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

        protected virtual void ShowIntent(Intent intent)
        {
            var activity = CurrentActivity;
            if (activity == null)
            {
                MvxTrace.Warning("Cannot Resolve current top activity");
                return;
            }
            activity.StartActivity(intent);
        }

        protected virtual void ShowFragment(
            MvxFragmentAttribute attribute,
            MvxViewModelRequest request)
        {
            var hostViewType = GetCurrentActivityViewModelType();
            var hostViewModelType = _viewModelToFragmentTypeMap[hostViewType];

            if(attribute.ParentActivityViewModelType != hostViewModelType)
            {
                var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(hostViewModelType);
                Show(hostViewModelRequest);
            }

            //var bundle = new Bundle();
            //var serializedRequest = Serializer.Serializer.SerializeObject(request);
            //bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

            //if (request is MvxViewModelInstanceRequest)
            //{
            //    Mvx.Resolve<IMvxChildViewModelCache>().Cache(((MvxViewModelInstanceRequest)request).ViewModelInstance);
            //}


            //var validHostViewModelType = 


            /*if (hostViewModelType != hostViewModelType)
            {
                Type newFragmentHostViewModelType = 

                var fragmentHostMvxViewModelRequest = MvxViewModelRequest.GetDefaultRequest(newFragmentHostViewModelType);
                ShowActivity(fragmentHostMvxViewModelRequest, request);
                return;
            }*/

            var fragmentName = FragmentJavaName(attribute.ViewType);
            var fragment = CreateFragment(fragmentName);

            var ft = CurrentActivity.FragmentManager.BeginTransaction();
            ft.Replace(attribute.FragmentContentId, fragment as Fragment, fragmentName);
            ft.CommitNowAllowingStateLoss();

            //TODO: Check if Activity host is already on screen
            //TODO: Check if Activity base extends FragmentActivity
            //TODO: Load Activity if not shown yet
            //TODO: Check if FragmentContentId can be found on Activity
            //TODO: Show fragment on Activity
        }

        protected IMvxFragmentView CreateFragment(string fragmentName)
        {
            var fragment = Fragment.Instantiate(CurrentActivity, fragmentName);
            return fragment as IMvxFragmentView;
        }

        protected Type GetCurrentActivityViewModelType()
        {
            Activity currentActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            Type currentActivityType = currentActivity.GetType();

            var activityViewModelType = _viewModelTypeFinder.FindTypeOrNull(currentActivityType);
            return activityViewModelType;
        }

        protected virtual void ShowDialogFragment(
            MvxDialogAttribute attribute,
            MvxViewModelRequest request)
        {
            var fragmentName = FragmentJavaName(attribute.ViewType);
            var dialog = Fragment.Instantiate(CurrentActivity, fragmentName) as DialogFragment;
            dialog.Show(CurrentActivity.FragmentManager, fragmentName);


            //var fragmentType = _fragmentHostRegistrationSettings.GetFragmentTypeAssociatedWith(request.ViewModelType);

            //var fragmentTag = GetFragmentTag(request);
            //FragmentCacheConfiguration.RegisterFragmentToCache(fragmentTag, fragmentType, request.ViewModelType, false);

            //TODO: handle show here
            //CreateFragment(fragmentType, null);

            //TODO: Check if class implements IDialogInterface
            //TODO: Check if class is a Fragment
            //TODO: Show as Dialog or DialogFragment
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            return Class.FromType(fragmentType).Name;
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

            var activity = CurrentActivity;

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
