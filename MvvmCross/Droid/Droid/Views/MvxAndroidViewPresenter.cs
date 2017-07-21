using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter : MvxViewPresenter, IMvxAndroidViewPresenter
    {
        protected virtual Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        protected virtual FragmentManager CurrentFragmentManager => CurrentActivity.FragmentManager;
        protected virtual IMvxAndroidActivityLifetimeListener ActivityLifetimeListener { get; set; } = Mvx.Resolve<IMvxAndroidActivityLifetimeListener>();

        protected IEnumerable<Assembly> _androidViewAssemblies;
        protected ConditionalWeakTable<MvxBasePresentationAttribute, IMvxFragmentView> _cachedFragments = new ConditionalWeakTable<MvxBasePresentationAttribute, IMvxFragmentView>();
        private ConditionalWeakTable<IMvxViewModel, DialogFragment> _dialogs = new ConditionalWeakTable<IMvxViewModel, DialogFragment>();

        protected Lazy<IMvxNavigationSerializer> _lazyNavigationSerializerFactory;
        protected IMvxNavigationSerializer Serializer;
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";
        protected MvxViewModelRequest _pendingRequest;

        protected IMvxViewModelTypeFinder _viewModelTypeFinder { get; } = Mvx.Resolve<IMvxViewModelTypeFinder>();

        private IMvxViewsContainer _viewsContainer;
        protected IMvxViewsContainer ViewsContainer
        {
            get
            {
                if(_viewsContainer == null)
                    _viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
        }

        private Dictionary<Type, Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;
        protected Dictionary<Type, Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest>> AttributeTypesToShowMethodDictionary
        {
            get
            {
                if (_attributeTypesToShowMethodDictionary == null)
                {
                    _attributeTypesToShowMethodDictionary = new Dictionary<Type, Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest>>();
                    RegisterAttributeTypes();
                }
                return _attributeTypesToShowMethodDictionary;
            }
        }

        private Dictionary<Type, IList<MvxBasePresentationAttribute>> _viewModelToPresentationAttributeMap;
        protected Dictionary<Type, IList<MvxBasePresentationAttribute>> ViewModelToPresentationAttributeMap
        {
            get
            {
                if (_viewModelToPresentationAttributeMap == null)
                {
                    _viewModelToPresentationAttributeMap = new Dictionary<Type, IList<MvxBasePresentationAttribute>>();
                    RegisterAttributes();
                }
                return _viewModelToPresentationAttributeMap;
            }
        }

        public MvxAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies)
        {
            _androidViewAssemblies = androidViewAssemblies;
            ActivityLifetimeListener.ActivityChanged += ActivityLifetimeListener_ActivityChanged;
        }

        protected virtual void ActivityLifetimeListener_ActivityChanged(object sender, MvxActivityEventArgs e)
        {
            if (e.ActivityState == MvxActivityState.OnResume && _pendingRequest != null)
            {
                Show(_pendingRequest);
                _pendingRequest = null;
            }
            else if(e.ActivityState == MvxActivityState.OnCreate && e.Extras is Bundle savedBundle)
            {
                //TODO: Restore fragments from bundle
            }
            else if(e.ActivityState == MvxActivityState.OnSaveInstanceState && e.Extras is Bundle outBundle)
            {
                //TODO: Save fragments into bundle
            }
            else if(e.ActivityState == MvxActivityState.OnDestroy)
            {
                //TODO: Should be check for Fragments on this Activity and destroy them?
            }
        }

        private void RegisterAttributes()
        {
            var typesWithBasePresentationAttribute = _androidViewAssemblies
                         .SelectMany(x => x.DefinedTypes)
                         .Select(x => x.AsType())
                         .Where(x => x.HasBasePresentationAttribute())
                         .ToList();

            foreach (var typeWithAttribute in typesWithBasePresentationAttribute)
            {
                var viewModelType = GetAssociatedViewModelType(typeWithAttribute);

                if (!ViewModelToPresentationAttributeMap.ContainsKey(viewModelType))
                    ViewModelToPresentationAttributeMap.Add(viewModelType, new List<MvxBasePresentationAttribute>());

                foreach (var attribute in typeWithAttribute.GetBasePresentationAttributes())
                {
                    //TODO: Can we set the viewType from somewhere else?
                    attribute.ViewType = typeWithAttribute;
                    ViewModelToPresentationAttributeMap[viewModelType].Add(attribute);
                }
            }
        }

        protected Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = _viewModelTypeFinder.FindTypeOrNull(fromFragmentType);
            return viewModelType ?? fromFragmentType.GetBasePresentationAttributes().First().ViewModelType;
        }

        protected virtual void RegisterAttributeTypes()
        {
            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxActivityAttribute),
               (view, attribute, request) => ShowActivity(view, (MvxActivityAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxFragmentAttribute),
               (view, attribute, request) => ShowFragment(view, (MvxFragmentAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
               typeof(MvxDialogAttribute),
               (view, attribute, request) => ShowDialogFragment(view, (MvxDialogAttribute)attribute, request));
        }

        protected virtual MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType)
        {
            IList<MvxBasePresentationAttribute> attributes;
            if (ViewModelToPresentationAttributeMap.TryGetValue(viewModelType, out attributes))
            {
                var attribute = attributes.FirstOrDefault();
                if (attribute.ViewType?.GetInterfaces().OfType<IMvxOverridePresentationAttribute>().FirstOrDefault() is IMvxOverridePresentationAttribute view)
                {
                    var presentationAttribute = view.PresentationAttribute();

                    if (presentationAttribute != null)
                        return presentationAttribute;
                }
                return attribute;
            }

            var viewType = ViewsContainer.GetViewType(viewModelType);
            if (viewType.GetInterfaces().Contains(typeof(IDialogInterface)))
                return new MvxDialogAttribute();
            if (viewType.IsSubclassOf(typeof(Fragment)))
                return new MvxFragmentAttribute(GetCurrentActivityViewModelType(), Android.Resource.Id.Content);

            return new MvxActivityAttribute() { ViewModelType = viewModelType };
        }

        protected Type GetCurrentActivityViewModelType()
        {
            Type currentActivityType = CurrentActivity.GetType();

            var activityViewModelType = _viewModelTypeFinder.FindTypeOrNull(currentActivityType);
            return activityViewModelType;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var attribute = GetAttributeForViewModel(request.ViewModelType);
            attribute.ViewModelType = request.ViewModelType;
            var view = attribute.ViewType;
            var attributeType = attribute.GetType();

            if (AttributeTypesToShowMethodDictionary.TryGetValue(attributeType,
                out Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest> showAction))
            {
                showAction.Invoke(view, attribute, request);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        protected virtual void ShowActivity(
            Type view,
            MvxActivityAttribute attribute,
            MvxViewModelRequest request)
        {
            var intent = CreateIntentForRequest(request);
            if (attribute.Extras != null)
                intent.PutExtras(attribute.Extras);
            ShowIntent(intent);
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

        protected virtual void ShowHostActivity(MvxFragmentAttribute attribute)
        {
            var viewType = ViewsContainer.GetViewType(attribute.ActivityHostViewModelType);
            if (!viewType.IsSubclassOf(typeof(Activity)))
                throw new MvxException("The host activity doesnt inherit Activity");

            var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            Show(hostViewModelRequest);
        }

        protected virtual void ShowFragment(
            Type view,
            MvxFragmentAttribute attribute,
            MvxViewModelRequest request)
        {
            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                _pendingRequest = request;
                ShowHostActivity(attribute);
            }
            else
            {
                if (CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
                    throw new NullReferenceException("FrameLayout to show Fragment not found");

                var fragmentName = FragmentJavaName(attribute.ViewType);
                var fragment = CreateFragment(attribute, fragmentName);

                //TODO: Find a better way to set the ViewModel at the Fragment
                if (request is MvxViewModelInstanceRequest instanceRequest)
                    fragment.ViewModel = instanceRequest.ViewModelInstance;
                else
                {
                    fragment.ViewModel = (IMvxViewModel)Mvx.IocConstruct(request.ViewModelType);
                }

                var ft = CurrentActivity.FragmentManager.BeginTransaction();

                if (attribute.SharedElements != null)
                {
                    foreach (var item in attribute.SharedElements)
                    {
                        ft.AddSharedElement(item.Value, item.Key);
                    }
                }
                if (!attribute.CustomAnimations.Equals((int.MinValue, int.MinValue, int.MinValue, int.MinValue)))
                {
                    var customAnimations = attribute.CustomAnimations;
                    ft.SetCustomAnimations(customAnimations.enter, customAnimations.exit, customAnimations.popEnter, customAnimations.popExit);
                }
                if (attribute.TransitionStyle != int.MinValue)
                    ft.SetTransitionStyle(attribute.TransitionStyle);

                ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
                ft.CommitNowAllowingStateLoss();
            }
        }

        protected virtual void ShowDialogFragment(
            Type view,
            MvxDialogAttribute attribute,
            MvxViewModelRequest request)
        {
            var fragmentName = FragmentJavaName(attribute.ViewType);
            var dialog = (DialogFragment)CreateFragment(attribute, fragmentName);

            //TODO: Find a better way to set the ViewModel at the Fragment
            IMvxViewModel viewModel;
            if (request is MvxViewModelInstanceRequest instanceRequest)
                viewModel = instanceRequest.ViewModelInstance;
            else
            {
                viewModel = (IMvxViewModel)Mvx.IocConstruct(request.ViewModelType);
            }
            ((IMvxFragmentView)dialog).ViewModel = viewModel;
            dialog.Cancelable = attribute.Cancelable;

            _dialogs.Add(viewModel, dialog);
            dialog.Show(CurrentFragmentManager, fragmentName);
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
            var attribute = GetAttributeForViewModel(viewModel.GetType());

            if (attribute is MvxActivityAttribute)
            {
                //TODO: Find something to close the dialogs

                if (CurrentFragmentManager.BackStackEntryCount > 0)
                    CurrentFragmentManager.PopBackStackImmediate(null, PopBackStackFlags.Inclusive);

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
            else if (attribute is MvxFragmentAttribute fragment)
            {
                if (CurrentFragmentManager.BackStackEntryCount > 0)
                {
                    var fragmentName = FragmentJavaName(attribute.ViewType);
                    CurrentFragmentManager.PopBackStackImmediate(fragmentName, PopBackStackFlags.Inclusive);
                }
                else
                    CurrentActivity.Finish();
            }
            else if (attribute is MvxDialogAttribute dialogAttribute)
            {
                if (_dialogs.TryGetValue(viewModel, out DialogFragment dialog))
                {
                    dialog.DismissAllowingStateLoss();
                    dialog.Dispose();
                    _dialogs.Remove(viewModel);
                }
            }
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            return Class.FromType(fragmentType).Name;
        }

        protected virtual IMvxFragmentView CreateFragment(MvxBasePresentationAttribute attribute, 
            string fragmentName)
        {
            try
            {
                IMvxFragmentView fragment;
                if (attribute is MvxFragmentAttribute fragmentAttribute && fragmentAttribute.IsCacheableFragment)
                {
                    if (_cachedFragments.TryGetValue(attribute, out fragment))
                        return fragment;

                    fragment = (IMvxFragmentView)Fragment.Instantiate(CurrentActivity, fragmentName);
                    _cachedFragments.Add(attribute, fragment);
                }
                else
                    fragment = (IMvxFragmentView)Fragment.Instantiate(CurrentActivity, fragmentName);
                return fragment;
            }
            catch
            {
                throw new MvxException($"Cannot create Fragment '{fragmentName}'. Use the MvxAppCompatViewPresenter when using Android Support Fragments");
            }
        }
    }
}
