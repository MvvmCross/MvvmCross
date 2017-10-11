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
using MvvmCross.Droid.Views.Fragments;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Views
{
    public class MvxAndroidViewPresenter : MvxViewPresenter, IMvxAndroidViewPresenter, IMvxAttributeViewPresenter
    {
        protected IEnumerable<Assembly> AndroidViewAssemblies { get; set; }
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";
        protected MvxViewModelRequest _pendingRequest;

        protected virtual FragmentManager CurrentFragmentManager => CurrentActivity.FragmentManager;

        protected virtual ConditionalWeakTable<IMvxViewModel, DialogFragment> Dialogs { get; } = new ConditionalWeakTable<IMvxViewModel, DialogFragment>();

        private IMvxAndroidCurrentTopActivity _mvxAndroidCurrentTopActivity;
        protected virtual Activity CurrentActivity
        {
            get
            {
                if (_mvxAndroidCurrentTopActivity == null)
                    _mvxAndroidCurrentTopActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                return _mvxAndroidCurrentTopActivity.Activity;
            }
        }

        private IMvxAndroidActivityLifetimeListener _activityLifetimeListener;
        protected IMvxAndroidActivityLifetimeListener ActivityLifetimeListener
        {
            get
            {
                if (_activityLifetimeListener == null)
                    _activityLifetimeListener = Mvx.Resolve<IMvxAndroidActivityLifetimeListener>();
                return _activityLifetimeListener;
            }
        }

        private IMvxViewModelTypeFinder _viewModelTypeFinder;
        public IMvxViewModelTypeFinder ViewModelTypeFinder
        {
            get
            {
                if (_viewModelTypeFinder == null)
                    _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
                return _viewModelTypeFinder;
            }
        }

        private IMvxViewsContainer _viewsContainer;
        public IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
        }

        private IMvxNavigationSerializer _navigationSerializer;
        protected IMvxNavigationSerializer NavigationSerializer
        {
            get
            {
                if (_navigationSerializer == null)
                    _navigationSerializer = Mvx.Resolve<IMvxNavigationSerializer>();
                return _navigationSerializer;
            }
        }

        private Dictionary<Type, MvxPresentationAttributeAction> _attributeTypesActionsDictionary;
        public Dictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary
        {
            get
            {
                if (_attributeTypesActionsDictionary == null)
                {
                    _attributeTypesActionsDictionary = new Dictionary<Type, MvxPresentationAttributeAction>();
                    RegisterAttributeTypes();
                }
                return _attributeTypesActionsDictionary;
            }
        }

        private Dictionary<Type, IList<MvxBasePresentationAttribute>> _viewModelToPresentationAttributeMap;
        public Dictionary<Type, IList<MvxBasePresentationAttribute>> ViewModelToPresentationAttributeMap
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
            AndroidViewAssemblies = androidViewAssemblies;
            ActivityLifetimeListener.ActivityChanged += ActivityLifetimeListener_ActivityChanged;
        }

        protected virtual void ActivityLifetimeListener_ActivityChanged(object sender, MvxActivityEventArgs e)
        {
            if (e.ActivityState == MvxActivityState.OnResume && _pendingRequest != null)
            {
                Show(_pendingRequest);
                _pendingRequest = null;
            }
            else if (e.ActivityState == MvxActivityState.OnCreate && e.Extras is Bundle savedBundle)
            {
                //TODO: Restore fragments from bundle
            }
            else if (e.ActivityState == MvxActivityState.OnSaveInstanceState && e.Extras is Bundle outBundle)
            {
                //TODO: Save fragments into bundle
            }
            else if (e.ActivityState == MvxActivityState.OnDestroy)
            {
                //TODO: Should be check for Fragments on this Activity and destroy them?
            }
        }

        private void RegisterAttributes()
        {
            var typesWithBasePresentationAttribute = AndroidViewAssemblies
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
                    if (attribute.ViewType == null)
                        attribute.ViewType = typeWithAttribute;
                    ViewModelToPresentationAttributeMap[viewModelType].Add(attribute);
                }
            }
        }

        protected Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = ViewModelTypeFinder.FindTypeOrNull(fromFragmentType);
            return viewModelType ?? fromFragmentType.GetBasePresentationAttributes().First().ViewModelType;
        }

        public virtual void RegisterAttributeTypes()
        {
            _attributeTypesActionsDictionary.Add(
                typeof(MvxActivityPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowActivity(view, (MvxActivityPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseActivity(viewModel, (MvxActivityPresentationAttribute)attribute)
                });

            _attributeTypesActionsDictionary.Add(
                typeof(MvxFragmentPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowFragment(view, (MvxFragmentPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseFragment(viewModel, (MvxFragmentPresentationAttribute)attribute)
                });

            _attributeTypesActionsDictionary.Add(
                typeof(MvxDialogFragmentPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowDialogFragment(view, (MvxDialogFragmentPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseFragmentDialog(viewModel, (MvxDialogFragmentPresentationAttribute)attribute)
                });
        }

        public virtual MvxBasePresentationAttribute GetPresentationAttribute(Type viewModelType)
        {
            var viewType = ViewsContainer.GetViewType(viewModelType);

            var overrideAttribute = GetOverridePresentationAttribute(viewModelType, viewType);
            if (overrideAttribute != null)
                return overrideAttribute;

            IList<MvxBasePresentationAttribute> attributes;
            if (ViewModelToPresentationAttributeMap.TryGetValue(viewModelType, out attributes))
            {
                MvxBasePresentationAttribute attribute = null;

                if (attributes.Count > 1)
                {
                    var fragmentAttributes = attributes.OfType<MvxFragmentPresentationAttribute>();

                    // check if fragment can be displayed as child fragment first
                    foreach (var item in fragmentAttributes.Where(att => att.FragmentHostViewType != null))
                    {
                        var fragment = GetFragmentByViewType(item.FragmentHostViewType);

                        // if the fragment exists, and is on top, then use the current attribute 
                        if (fragment != null && fragment.IsVisible && fragment.View.FindViewById(item.FragmentContentId) != null)
                        {
                            attribute = item;
                            break;
                        }
                    }

                    // if attribute is still null, check if fragment can be displayed in current activity
                    if (attribute == null)
                    {
                        var currentActivityHostViewModelType = GetCurrentActivityViewModelType();
                        foreach (var item in fragmentAttributes.Where(att => att.ActivityHostViewModelType != null))
                        {
                            if (CurrentActivity.FindViewById(item.FragmentContentId) != null && item.ActivityHostViewModelType == currentActivityHostViewModelType)
                            {
                                attribute = item;
                                break;
                            }
                        }
                    }
                }

                if (attribute == null)
                    attribute = attributes.FirstOrDefault();

                return attribute;
            }

            return CreatePresentationAttribute(viewModelType, viewType);
        }

        public virtual MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType.IsSubclassOf(typeof(DialogFragment)))
            {
                MvxTrace.Trace("PresentationAttribute not found for {0}. Assuming DialogFragment presentation", viewType.Name);
                return new MvxDialogFragmentPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }

            if (viewType.IsSubclassOf(typeof(Fragment)))
            {
                MvxTrace.Trace("PresentationAttribute not found for {0}. Assuming Fragment presentation", viewType.Name);
                return new MvxFragmentPresentationAttribute(GetCurrentActivityViewModelType(), Android.Resource.Id.Content) { ViewType = viewType, ViewModelType = viewModelType };
            }

            MvxTrace.Trace("PresentationAttribute not found for {0}. Assuming Activity presentation", viewType.Name);
            return new MvxActivityPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
        }

        public virtual MvxBasePresentationAttribute GetOverridePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType?.GetInterface(nameof(IMvxOverridePresentationAttribute)) != null)
            {
                var viewInstance = Activator.CreateInstance(viewType) as Java.Lang.Object;
                using (viewInstance)
                {
                    var presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute();

                    if (presentationAttribute == null)
                    {
                        MvxTrace.Warning("Override PresentationAttribute null. Falling back to existing attribute.");
                    }
                    else
                    {
                        if (presentationAttribute.ViewType == null)
                            presentationAttribute.ViewType = viewType;

                        if (presentationAttribute.ViewModelType == null)
                            presentationAttribute.ViewModelType = viewModelType;

                        return presentationAttribute;
                    }
                }
            }

            return null;
        }

        protected Type GetCurrentActivityViewModelType()
        {
            Type currentActivityType = CurrentActivity.GetType();

            var activityViewModelType = ViewModelTypeFinder.FindTypeOrNull(currentActivityType);
            return activityViewModelType;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var attribute = GetPresentationAttribute(request.ViewModelType);
            attribute.ViewModelType = request.ViewModelType;
            var viewType = attribute.ViewType;
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.ShowAction == null)
                    throw new NullReferenceException($"attributeAction.ShowAction is null for attribute: {attributeType.Name}");

                attributeAction.ShowAction.Invoke(viewType, attribute, request);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        #region Show implementations
        protected virtual void ShowActivity(
            Type view,
            MvxActivityPresentationAttribute attribute,
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

            if (request is MvxViewModelInstanceRequest viewModelInstanceRequest)
            {
                var instanceRequest = requestTranslator.GetIntentWithKeyFor(viewModelInstanceRequest.ViewModelInstance);
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

        protected virtual void ShowHostActivity(MvxFragmentPresentationAttribute attribute)
        {
            var viewType = ViewsContainer.GetViewType(attribute.ActivityHostViewModelType);
            if (!viewType.IsSubclassOf(typeof(Activity)))
                throw new MvxException("The host activity doesn't inherit Activity");

            var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            hostViewModelRequest.PresentationValues = _pendingRequest.PresentationValues;
            Show(hostViewModelRequest);
        }

        protected virtual void ShowFragment(
            Type view,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // if attribute has a Fragment Host, then show it as nested and return
            if (attribute.FragmentHostViewType != null)
            {
                ShowNestedFragment(view, attribute, request);

                return;
            }

            // if there is no Actitivty host associated, assume is the current activity
            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                MvxTrace.Trace("Activity host with ViewModelType {0} is not CurrentTopActivity. Showing Activity before showing Fragment for {1}",
                    attribute.ActivityHostViewModelType, attribute.ViewModelType);
                _pendingRequest = request;
                ShowHostActivity(attribute);
            }
            else
            {
                if (CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
                    throw new NullReferenceException("FrameLayout to show Fragment not found");

                PerformShowFragmentTransaction(CurrentActivity.FragmentManager, attribute, request);
            }
        }

        protected virtual void ShowNestedFragment(
            Type view,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // current implementation only supports one level of nesting 

            var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
            if (fragmentHost == null)
                throw new NullReferenceException($"Fragment host not found when trying to show View {view.Name} as Nested Fragment");

            if (!fragmentHost.IsVisible)
                throw new InvalidOperationException($"Fragment host is not visible when trying to show View {view.Name} as Nested Fragment");

            PerformShowFragmentTransaction(fragmentHost.ChildFragmentManager, attribute, request);
        }

        protected virtual void PerformShowFragmentTransaction(
            FragmentManager fragmentManager,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var fragmentName = FragmentJavaName(attribute.ViewType);

            IMvxFragmentView fragment = null;
            if (attribute.IsCacheableFragment)
            {
                fragment = (IMvxFragmentView)fragmentManager.FindFragmentByTag(fragmentName);
            }
            fragment = fragment ?? CreateFragment(attribute, fragmentName);

            var fragmentView = fragment.ToFragment();

            // MvxNavigationService provides an already instantiated ViewModel here,
            // therefore just assign it
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                fragment.ViewModel = instanceRequest.ViewModelInstance;
            }
            else
            {
                var bundle = new Bundle();
                var serializedRequest = NavigationSerializer.Serializer.SerializeObject(request);
                bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

                if (fragmentView != null)
                {
                    if (fragmentView.Arguments == null)
                    {
                        fragmentView.Arguments = bundle;
                    }
                    else
                    {
                        fragmentView.Arguments.Clear();
                        fragmentView.Arguments.PutAll(bundle);
                    }
                }
            }

            var ft = fragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, attribute);

            if (attribute.AddToBackStack == true)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, fragmentView, attribute);

            ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
            ft.CommitAllowingStateLoss();

            OnFragmentChanged(ft, fragmentView, attribute);
        }

        protected virtual void OnBeforeFragmentChanging(FragmentTransaction ft, MvxFragmentPresentationAttribute attribute)
        {
            if (attribute.SharedElements != null)
            {
                foreach (var item in attribute.SharedElements)
                {
                    ft.AddSharedElement(item.Value, item.Key);
                }
            }

            if (!attribute.EnterAnimation.Equals(int.MinValue) && !attribute.ExitAnimation.Equals(int.MinValue))
            {
                if (!attribute.PopEnterAnimation.Equals(int.MinValue) && !attribute.PopExitAnimation.Equals(int.MinValue))
                    ft.SetCustomAnimations(attribute.EnterAnimation, attribute.ExitAnimation, attribute.PopEnterAnimation, attribute.PopExitAnimation);
                else
                    ft.SetCustomAnimations(attribute.EnterAnimation, attribute.ExitAnimation);
            }

            if (attribute.TransitionStyle != int.MinValue)
                ft.SetTransitionStyle(attribute.TransitionStyle);
        }

        protected virtual void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
        {

        }

        protected virtual void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
        {

        }

        protected virtual void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
        {

        }

        protected virtual void ShowDialogFragment(
            Type view,
            MvxDialogFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var fragmentName = FragmentJavaName(attribute.ViewType);
            IMvxFragmentView mvxFragmentView = CreateFragment(attribute, fragmentName);
            var dialog = (DialogFragment)mvxFragmentView;

            // MvxNavigationService provides an already instantiated ViewModel here,
            // therefore just assign it
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                mvxFragmentView.ViewModel = instanceRequest.ViewModelInstance;
            }
            else
            {
                mvxFragmentView.LoadViewModelFrom(request, null);
            }

            dialog.Cancelable = attribute.Cancelable;

            Dialogs.Add(mvxFragmentView.ViewModel, dialog);

            var ft = CurrentFragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, attribute);

            if (attribute.AddToBackStack == true)
                ft.AddToBackStack(fragmentName);

            dialog.Show(ft, fragmentName);
        }
        #endregion

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
            var attribute = GetPresentationAttribute(viewModel.GetType());
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.CloseAction == null)
                    throw new NullReferenceException($"attributeAction.CloseAction is null for attribute: {attributeType.Name}");

                attributeAction.CloseAction.Invoke(viewModel, attribute);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        #region Close implementations
        protected virtual bool CloseActivity(IMvxViewModel viewModel, MvxActivityPresentationAttribute attribute)
        {
            var currentView = CurrentActivity as IMvxView;

            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return false;
            }

            if (currentView.ViewModel != viewModel)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return false;
            }

            CurrentActivity.Finish();

            return true;
        }

        protected virtual bool CloseFragmentDialog(IMvxViewModel viewModel, MvxDialogFragmentPresentationAttribute attribute)
        {
            if (Dialogs.TryGetValue(viewModel, out DialogFragment dialog))
            {
                dialog.DismissAllowingStateLoss();
                dialog.Dispose();
                Dialogs.Remove(viewModel);

                return true;
            }
            return false;
        }

        protected virtual bool CloseFragment(IMvxViewModel viewModel, MvxFragmentPresentationAttribute attribute)
        {
            // try to close nested fragment first
            if (attribute.FragmentHostViewType != null)
            {
                var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragmentHost != null
                    && TryPerformCloseFragmentTransaction(fragmentHost.ChildFragmentManager, attribute))
                    return true;
            }

            // Close fragment. If it isn't successful, then close the current Activity
            if (TryPerformCloseFragmentTransaction(CurrentFragmentManager, attribute))
            {
                return true;
            }
            else
            {
                CurrentActivity.Finish();
                return true;
            }
        }

        protected virtual bool TryPerformCloseFragmentTransaction(
            FragmentManager fragmentManager,
            MvxFragmentPresentationAttribute fragmentAttribute)
        {
            if (fragmentManager.BackStackEntryCount > 0)
            {
                var fragmentName = FragmentJavaName(fragmentAttribute.ViewType);
                fragmentManager.PopBackStackImmediate(fragmentName, PopBackStackFlags.Inclusive);

                OnFragmentPopped(null, null, fragmentAttribute);
                return true;
            }
            else if (CurrentFragmentManager.FindFragmentByTag(fragmentAttribute.ViewType.Name) != null)
            {
                var ft = fragmentManager.BeginTransaction();
                var fragment = fragmentManager.FindFragmentByTag(fragmentAttribute.ViewType.Name);

                if (!fragmentAttribute.EnterAnimation.Equals(int.MinValue) && !fragmentAttribute.ExitAnimation.Equals(int.MinValue))
                {
                    if (!fragmentAttribute.PopEnterAnimation.Equals(int.MinValue) && !fragmentAttribute.PopExitAnimation.Equals(int.MinValue))
                        ft.SetCustomAnimations(fragmentAttribute.EnterAnimation, fragmentAttribute.ExitAnimation, fragmentAttribute.PopEnterAnimation, fragmentAttribute.PopExitAnimation);
                    else
                        ft.SetCustomAnimations(fragmentAttribute.EnterAnimation, fragmentAttribute.ExitAnimation);
                }
                if (fragmentAttribute.TransitionStyle != int.MinValue)
                    ft.SetTransitionStyle(fragmentAttribute.TransitionStyle);

                ft.Remove(fragment);
                ft.CommitAllowingStateLoss();

                OnFragmentPopped(ft, fragment, fragmentAttribute);
                return true;
            }
            return false;
        }
        #endregion

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            return Class.FromType(fragmentType).Name;
        }

        protected virtual IMvxFragmentView CreateFragment(MvxBasePresentationAttribute attribute,
            string fragmentName)
        {
            try
            {
                var fragment = (IMvxFragmentView)Fragment.Instantiate(CurrentActivity, fragmentName);
                return fragment;
            }
            catch
            {
                throw new MvxException($"Cannot create Fragment '{fragmentName}'. Use the MvxAppCompatViewPresenter when using Android Support Fragments");
            }
        }

        protected virtual Fragment GetFragmentByViewType(Type type)
        {
            var fragmentName = FragmentJavaName(type);
            return CurrentFragmentManager.FindFragmentByTag(fragmentName);
        }
    }
}
