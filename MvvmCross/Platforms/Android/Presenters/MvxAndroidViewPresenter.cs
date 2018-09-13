// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Java.Lang;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Presenters
{
    public class MvxAndroidViewPresenter : MvxAttributeViewPresenter, IMvxAndroidViewPresenter
    {
        protected IEnumerable<Assembly> AndroidViewAssemblies { get; set; }
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";
        public const string SharedElementsBundleKey = "__sharedElementsKey";
        protected MvxViewModelRequest _pendingRequest;

        protected virtual FragmentManager CurrentFragmentManager => CurrentActivity.FragmentManager;
        
        private IMvxAndroidCurrentTopActivity _mvxAndroidCurrentTopActivity;
        protected virtual Activity CurrentActivity
        {
            get
            {
                if (_mvxAndroidCurrentTopActivity == null)
                    _mvxAndroidCurrentTopActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
                return _mvxAndroidCurrentTopActivity.Activity;
            }
        }

        private IMvxAndroidActivityLifetimeListener _activityLifetimeListener;
        protected IMvxAndroidActivityLifetimeListener ActivityLifetimeListener
        {
            get
            {
                if (_activityLifetimeListener == null)
                    _activityLifetimeListener = Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>();
                return _activityLifetimeListener;
            }
        }

        private IMvxNavigationSerializer _navigationSerializer;
        protected IMvxNavigationSerializer NavigationSerializer
        {
            get
            {
                if (_navigationSerializer == null)
                    _navigationSerializer = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
                return _navigationSerializer;
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

        protected Type GetAssociatedViewModelType(Type fromFragmentType)
        {
            Type viewModelType = ViewModelTypeFinder.FindTypeOrNull(fromFragmentType);
            return viewModelType ?? fromFragmentType.GetBasePresentationAttributes().First().ViewModelType;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Add(
                typeof(MvxActivityPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowActivity(view, (MvxActivityPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseActivity(viewModel, (MvxActivityPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxFragmentPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowFragment(view, (MvxFragmentPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseFragment(viewModel, (MvxFragmentPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxDialogFragmentPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowDialogFragment(view, (MvxDialogFragmentPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseFragmentDialog(viewModel, (MvxDialogFragmentPresentationAttribute)attribute)
                });
        }

        public override MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request)
        {
            var viewType = ViewsContainer.GetViewType(request.ViewModelType);

            var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
            if (overrideAttribute != null)
                return overrideAttribute;

            IList<MvxBasePresentationAttribute> attributes = viewType.GetCustomAttributes<MvxBasePresentationAttribute>(true).ToList();
            if (attributes != null && attributes.Count > 0)
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

                attribute.ViewType = viewType;

                return attribute;
            }

            return CreatePresentationAttribute(request.ViewModelType, viewType);
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType.IsSubclassOf(typeof(DialogFragment)))
            {
                MvxLog.Instance.Trace("PresentationAttribute not found for {0}. Assuming DialogFragment presentation", viewType.Name);
                return new MvxDialogFragmentPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            else if (viewType.IsSubclassOf(typeof(Fragment)))
            {
                MvxLog.Instance.Trace("PresentationAttribute not found for {0}. Assuming Fragment presentation", viewType.Name);
                return new MvxFragmentPresentationAttribute(GetCurrentActivityViewModelType(), global::Android.Resource.Id.Content) { ViewType = viewType, ViewModelType = viewModelType };
            }
            else if (viewType.IsSubclassOf(typeof(Activity)))
            {
                MvxLog.Instance.Trace("PresentationAttribute not found for {0}. Assuming Activity presentation", viewType.Name);
                return new MvxActivityPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            return null;
        }
        
        protected Type GetCurrentActivityViewModelType()
        {
            Type currentActivityType = CurrentActivity?.GetType();

            var activityViewModelType = ViewModelTypeFinder.FindTypeOrNull(currentActivityType);
            return activityViewModelType;
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            return GetPresentationAttributeAction(request, out MvxBasePresentationAttribute attribute).ShowAction.Invoke(attribute.ViewType, attribute, request);
        }

        #region Show implementations
        protected virtual Task<bool> ShowActivity(
            Type view,
            MvxActivityPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var intent = CreateIntentForRequest(request);
            if (attribute.Extras != null)
                intent.PutExtras(attribute.Extras);

            ShowIntent(intent, CreateActivityTransitionOptions(intent, attribute, request));
            return Task.FromResult(true);
        }

        protected virtual Bundle CreateActivityTransitionOptions(Intent intent, MvxActivityPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var bundle = Bundle.Empty;

            if (CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity)
            {
                var elements = new List<string>();
                var transitionElementPairs = new List<Pair>();

                foreach (KeyValuePair<string, View> item in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
                {
                    var transitionName = item.Value.GetTransitionNameSupport();
                    if (!string.IsNullOrEmpty(transitionName))
                    {
                        transitionElementPairs.Add(Pair.Create(item.Value, transitionName));
                        elements.Add($"{item.Key}:{transitionName}");
                    }
                    else
                    {
                        MvxLog.Instance.Warn("A XML transitionName is required in order to transition a control when navigating.");
                    }
                }

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    var activityOptions = ActivityOptions.MakeSceneTransitionAnimation(CurrentActivity, transitionElementPairs.ToArray());
                    intent.PutExtra(SharedElementsBundleKey, string.Join("|", elements));
                    bundle = activityOptions.ToBundle();
                }
                else
                {
                    MvxLog.Instance.Warn("Shared element transition requires Android v21+.");
                }
            }

            return bundle;
        }

        protected virtual Intent CreateIntentForRequest(MvxViewModelRequest request)
        {
            IMvxAndroidViewModelRequestTranslator requestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();

            if (request is MvxViewModelInstanceRequest viewModelInstanceRequest)
            {
                var instanceRequest = requestTranslator.GetIntentWithKeyFor(viewModelInstanceRequest.ViewModelInstance);
                return instanceRequest.Item1;
            }
            return requestTranslator.GetIntentFor(request);
        }

        protected virtual void ShowIntent(Intent intent, Bundle bundle)
        {
            var activity = CurrentActivity;
            if (activity == null)
            {
                MvxLog.Instance.Warn("Cannot Resolve current top activity");
                return;
            }
            activity.StartActivity(intent, bundle);
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

        protected virtual Task<bool> ShowFragment(
            Type view,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // if attribute has a Fragment Host, then show it as nested and return
            if (attribute.FragmentHostViewType != null)
            {
                ShowNestedFragment(view, attribute, request);

                return Task.FromResult(true);
            }

            // if there is no Activity host associated, assume is the current activity
            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                MvxLog.Instance.Trace("Activity host with ViewModelType {0} is not CurrentTopActivity. Showing Activity before showing Fragment for {1}",
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
            return Task.FromResult(true);
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

            // MvxNavigationService provides an already instantiated ViewModel here
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                fragment.ViewModel = instanceRequest.ViewModelInstance;
            }

            // save MvxViewModelRequest in the Fragment's Arguments
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

            var ft = fragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, fragmentView, attribute, request);

            if (attribute.AddToBackStack == true)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, fragmentView, attribute, request);

            ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
            ft.CommitAllowingStateLoss();

            OnFragmentChanged(ft, fragmentView, attribute, request);
        }

        protected virtual void OnBeforeFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            if (CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity)
            {
                var elements = new List<string>();

                foreach (KeyValuePair<string, View> item in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
                {
                    var transitionName = item.Value.GetTransitionNameSupport();
                    if (!string.IsNullOrEmpty(transitionName))
                    {
                        ft.AddSharedElement(item.Value, transitionName);
                        elements.Add($"{item.Key}:{transitionName}");
                    }
                    else
                    {
                        MvxLog.Instance.Warn("A XML transitionName is required in order to transition a control when navigating.");
                    }
                }

                if (elements.Count > 0)
                    fragment.Arguments.PutString(SharedElementsBundleKey, string.Join("|", elements));
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

        protected virtual void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
        }

        protected virtual void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
        }

        protected virtual void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
        {
        }

        protected virtual Task<bool> ShowDialogFragment(
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

            var ft = CurrentFragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, dialog, attribute, request);

            if (attribute.AddToBackStack == true)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, dialog, attribute, request);

            dialog.Show(ft, fragmentName);

            OnFragmentChanged(ft, dialog, attribute, request);
            return Task.FromResult(true);
        }
        #endregion

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            return GetPresentationAttributeAction(new MvxViewModelInstanceRequest(viewModel), out MvxBasePresentationAttribute attribute).CloseAction.Invoke(viewModel, attribute);
        }

        #region Close implementations
        protected virtual Task<bool> CloseActivity(IMvxViewModel viewModel, MvxActivityPresentationAttribute attribute)
        {
            var currentView = CurrentActivity as IMvxView;

            if (currentView == null)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe has no current page");
                return Task.FromResult(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return Task.FromResult(false);
            }

            CurrentActivity.Finish();

            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseFragmentDialog(IMvxViewModel viewModel, MvxDialogFragmentPresentationAttribute attribute)
        {
            string tag = FragmentJavaName(attribute.ViewType);
            var toClose = CurrentFragmentManager.FindFragmentByTag(tag);
            if (toClose != null && toClose is DialogFragment dialog)
            {
                dialog.DismissAllowingStateLoss();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        protected virtual bool CloseFragments()
        {
            try
            {
                CurrentFragmentManager.PopBackStackImmediate();
            }
            catch (System.Exception ex)
            {
                MvxLog.Instance.Trace("Cannot close any fragments", ex);
            }
            return true;
        }

        protected virtual Task<bool> CloseFragment(IMvxViewModel viewModel, MvxFragmentPresentationAttribute attribute)
        {
            // try to close nested fragment first
            if (attribute.FragmentHostViewType != null)
            {
                var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragmentHost != null
                    && TryPerformCloseFragmentTransaction(fragmentHost.ChildFragmentManager, attribute))
                    return Task.FromResult(true);
            }

            // Close fragment. If it isn't successful, then close the current Activity
            if (TryPerformCloseFragmentTransaction(CurrentFragmentManager, attribute))
            {
                return Task.FromResult(true);
            }
            else
            {
                CurrentActivity.Finish();
                return Task.FromResult(true);
            }
        }

        protected virtual bool TryPerformCloseFragmentTransaction(
            FragmentManager fragmentManager,
            MvxFragmentPresentationAttribute fragmentAttribute)
        {
            var fragmentName = FragmentJavaName(fragmentAttribute.ViewType);

            if (fragmentManager.BackStackEntryCount > 0)
            {
                fragmentManager.PopBackStackImmediate(fragmentName, PopBackStackFlags.Inclusive);

                OnFragmentPopped(null, null, fragmentAttribute);
                return true;
            }
            else if (CurrentFragmentManager.FindFragmentByTag(fragmentName) != null)
            {
                var ft = fragmentManager.BeginTransaction();
                var fragment = fragmentManager.FindFragmentByTag(fragmentName);

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
            catch (System.Exception ex)
            {
                throw new MvxException(ex, $"Cannot create Fragment '{fragmentName}'. Use the MvxAppCompatViewPresenter when using Android Support Fragments");
            }
        }

        protected virtual Fragment GetFragmentByViewType(Type type)
        {
            var fragmentName = FragmentJavaName(type);
            var fragment = CurrentFragmentManager?.FindFragmentByTag(fragmentName);

            if (fragment != null)
            {
                return fragment;
            }

            return FindFragmentInChildren(fragmentName, CurrentFragmentManager);
        }

        protected virtual Fragment FindFragmentInChildren(string fragmentName, FragmentManager fragManager)
        {
            if(fragManager.BackStackEntryCount == 0)
                return null;

            for (int i = 0; i < fragManager.BackStackEntryCount; i++)
            {
                var parentFrag = fragManager.FindFragmentById(fragManager.GetBackStackEntryAt(i).Id);

                //let's try again finding it
                var frag = parentFrag?.ChildFragmentManager?.FindFragmentByTag(fragmentName);

                //if we found the frag lets return it!
                if (frag != null)
                {
                    return frag;
                }

                //reloop for other fragments
                FindFragmentInChildren(fragmentName, parentFrag?.ChildFragmentManager);
            }

            return null;
        }
    }
}
