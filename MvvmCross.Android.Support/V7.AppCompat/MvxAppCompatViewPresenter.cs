// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public class MvxAppCompatViewPresenter : MvxAndroidViewPresenter
    {
        public MvxAppCompatViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        protected new FragmentManager CurrentFragmentManager
        {
            get
            {
                if (CurrentActivity is FragmentActivity activity)
                    return activity.SupportFragmentManager;
                MvxAndroidLog.Instance.Trace("Cannot use Android Support Fragment within non AppCompat Activity");
                return null;
            }
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxTabLayoutPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowTabLayout(view, (MvxTabLayoutPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseViewPagerFragment(viewModel, (MvxViewPagerFragmentPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxViewPagerFragmentPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowViewPagerFragment(view, (MvxViewPagerFragmentPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseViewPagerFragment(viewModel, (MvxViewPagerFragmentPresentationAttribute)attribute)
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
                        var fragmentHost = GetFragmentByViewType(item.FragmentHostViewType);

                        // if the fragment exists, is on top, and (has the ContentId or the attribute is for ViewPager), then use it as current attribute 
                        if (fragmentHost != null 
                            && fragmentHost.IsVisible 
                            && (fragmentHost.View.FindViewById(item.FragmentContentId) != null || item is MvxViewPagerFragmentPresentationAttribute))
                        {
                            attribute = item;
                            break;
                        }
                    }

                    // if attribute is still null, check if fragment can be displayed in current activity
                    if (attribute == null)
                    {
                        var currentActivityHostViewModelType = GetCurrentActivityViewModelType();
                        foreach (var item in fragmentAttributes.Where(att => att.ActivityHostViewModelType != null && att.ActivityHostViewModelType == currentActivityHostViewModelType))
                        {
                            // check for MvxTabLayoutPresentationAttribute 
                            if (item is MvxTabLayoutPresentationAttribute tabLayoutAttribute
                                && CurrentActivity.FindViewById(tabLayoutAttribute.TabLayoutResourceId) != null)
                            {
                                attribute = item;
                                break;
                            }

                            // check for MvxViewPagerFragmentPresentationAttribute 
                            if (item is MvxViewPagerFragmentPresentationAttribute viewPagerAttribute
                                && CurrentActivity.FindViewById(viewPagerAttribute.ViewPagerResourceId) != null)
                            {
                                attribute = item;
                                break;
                            }

                            // check for MvxFragmentPresentationAttribute
                            if (CurrentActivity.FindViewById(item.FragmentContentId) != null)
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
                MvxAndroidLog.Instance.Trace("PresentationAttribute not found for {0}. Assuming DialogFragment presentation", viewType.Name);
                return new MvxDialogFragmentPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(Fragment)))
            {
                MvxAndroidLog.Instance.Trace("PresentationAttribute not found for {0}. Assuming Fragment presentation", viewType.Name);
                return new MvxFragmentPresentationAttribute(GetCurrentActivityViewModelType(), Android.Resource.Id.Content) { ViewType = viewType, ViewModelType = viewModelType };
            }

            return base.CreatePresentationAttribute(viewModelType, viewType);
        }

        public override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxPagePresentationHint pagePresentationHint)
            {
                var request = new MvxViewModelRequest(pagePresentationHint.ViewModel);
                var attribute = GetPresentationAttribute(request);

                if (attribute is MvxViewPagerFragmentPresentationAttribute pagerFragmentAttribute)
                {
                    var viewPager = FindViewPagerInFragmentPresentation(pagerFragmentAttribute);
                    if (viewPager?.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
                    {
                        var index = adapter.FragmentsInfo.FindIndex(f => f.Tag == pagerFragmentAttribute.Title);
                        if (index < 0)
                        {
                            MvxAndroidLog.Instance.Trace("Did not find ViewPager index for {0}, skipping presentation change...",
                                pagerFragmentAttribute.Title);

                            return Task.FromResult(false);
                        }

                        viewPager.SetCurrentItem(index, true);
                        return Task.FromResult(true);
                    }
                }
            }

            return base.ChangePresentation(hint);
        }

        private ViewPager FindViewPagerInFragmentPresentation(MvxViewPagerFragmentPresentationAttribute pagerFragmentAttribute)
        {
            ViewPager viewPager = null;

            // check for a ViewPager inside a Fragment
            if (pagerFragmentAttribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(pagerFragmentAttribute.FragmentHostViewType);
                viewPager = fragment.View.FindViewById<ViewPager>(pagerFragmentAttribute.ViewPagerResourceId);
            }

            // check for a ViewPager inside an Activity
            if (viewPager == null && pagerFragmentAttribute.ActivityHostViewModelType != null)
            {
                viewPager = CurrentActivity.FindViewById<ViewPager>(pagerFragmentAttribute.ViewPagerResourceId);
            }

            return viewPager;
        }

        #region Show implementations

        protected override void ShowHostActivity(MvxFragmentPresentationAttribute attribute)
        {
            var viewType = ViewsContainer.GetViewType(attribute.ActivityHostViewModelType);
            if (!viewType.IsSubclassOf(typeof(FragmentActivity)))
                throw new MvxException("The host activity doesn’t inherit FragmentActivity");

            var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            Show(hostViewModelRequest);
        }

        protected override Task<bool> ShowFragment(Type view,
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
                MvxAndroidLog.Instance.Trace("Activity host with ViewModelType {0} is not CurrentTopActivity. Showing Activity before showing Fragment for {1}",
                    attribute.ActivityHostViewModelType, attribute.ViewModelType);
                _pendingRequest = request;
                ShowHostActivity(attribute);
            }
            else
            {
                if (CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
                    throw new NullReferenceException("FrameLayout to show Fragment not found");

                PerformShowFragmentTransaction(CurrentFragmentManager, attribute, request);
            }
            return Task.FromResult(true);
        }

        protected override void ShowNestedFragment(Type view, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
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
                    var transitionName = ViewCompat.GetTransitionName(item.Value);
                    if (!string.IsNullOrEmpty(transitionName))
                    {
                        ft.AddSharedElement(item.Value, transitionName);
                        elements.Add($"{item.Key}:{transitionName}");
                    }
                    else
                    {
                        MvxAndroidLog.Instance.Warn("A XML transitionName is required in order to transition a control when navigating.");
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

        protected override Task<bool> ShowDialogFragment(Type view,
           MvxDialogFragmentPresentationAttribute attribute,
           MvxViewModelRequest request)
        {
            var fragmentName = FragmentJavaName(attribute.ViewType);
            IMvxFragmentView mvxFragmentView = CreateFragment(attribute, fragmentName);
            var dialog = mvxFragmentView as DialogFragment;
            if (dialog == null)
            {
                throw new MvxException("Fragment {0} does not extend {1}", fragmentName, typeof(DialogFragment).FullName);
            }

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

        protected virtual Task<bool> ShowViewPagerFragment(
            Type view,
            MvxViewPagerFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // if the attribute doesn't supply any host, assume current activity!
            if (attribute.FragmentHostViewType == null && attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            ViewPager viewPager = null;
            FragmentManager fragmentManager = null;

            // check for a ViewPager inside a Fragment
            if(attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
                if(fragment == null)
                    throw new MvxException("Fragment not found", attribute.FragmentHostViewType.Name);

                if(fragment.View == null)
                    throw new MvxException("Fragment.View is null. Please consider calling Navigate later in your code", attribute.FragmentHostViewType.Name);

                viewPager = fragment.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = fragment.ChildFragmentManager;
            }

            // check for a ViewPager inside an Activity
            if(attribute.ActivityHostViewModelType != null)
            {
                var currentActivityViewModelType = GetCurrentActivityViewModelType();

                // if the host Activity is not the top-most Activity, then show it before proceeding, and return false for now
                if(attribute.ActivityHostViewModelType != currentActivityViewModelType)
                {
                    _pendingRequest = request;
                    ShowHostActivity(attribute);
                    return Task.FromResult(false);
                }

                viewPager = CurrentActivity.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = CurrentFragmentManager;
            }

            // no more cases to check. Just throw if ViewPager wasn't found
            if (viewPager == null)
                throw new MvxException("ViewPager not found");

            if (viewPager.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
            {
                if (request is MvxViewModelInstanceRequest instanceRequest)
                    adapter.FragmentsInfo.Add(new MvxViewPagerFragmentInfo(attribute.Title, attribute.ViewType, instanceRequest.ViewModelInstance));
                else
                    adapter.FragmentsInfo.Add(new MvxViewPagerFragmentInfo(attribute.Title, attribute.ViewType, attribute.ViewModelType));

                adapter.NotifyDataSetChanged();
            }
            else
            {
                var fragments = new List<MvxViewPagerFragmentInfo>();
                if (request is MvxViewModelInstanceRequest instanceRequest)
                    fragments.Add(new MvxViewPagerFragmentInfo(attribute.Title, attribute.ViewType, instanceRequest.ViewModelInstance));
                else
                    fragments.Add(new MvxViewPagerFragmentInfo(attribute.Title, attribute.ViewType, attribute.ViewModelType));

                viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(CurrentActivity, fragmentManager, fragments);
            }
        
            return Task.FromResult(true);
        }
        
        protected virtual async Task<bool> ShowTabLayout(
            Type view,
            MvxTabLayoutPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (!await ShowViewPagerFragment(view, attribute, request))
                return false;
            
            ViewPager viewPager = null;
            TabLayout tabLayout = null;

            // check for a ViewPager inside a Fragment
            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);

                viewPager = fragment.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = fragment.View.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            // check for a ViewPager inside an Activity
            if (attribute.ActivityHostViewModelType != null)
            {
                viewPager = CurrentActivity.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = CurrentActivity.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            if (viewPager != null && tabLayout != null)
            {
                tabLayout.SetupWithViewPager(viewPager);
                return true;
            }

            throw new MvxException("ViewPager or TabLayout not found");
        }
        #endregion

        #region Close implementations
        protected override Task<bool> CloseFragmentDialog(IMvxViewModel viewModel, MvxDialogFragmentPresentationAttribute attribute)
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

        protected virtual Task<bool> CloseViewPagerFragment(IMvxViewModel viewModel, MvxViewPagerFragmentPresentationAttribute attribute)
        {
            ViewPager viewPager = null;
            FragmentManager fragmentManager = null;

            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragment == null)
                    throw new MvxException("Fragment not found", attribute.FragmentHostViewType.Name);

                viewPager = fragment.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = fragment.ChildFragmentManager;
            }
            else
            {
                viewPager = CurrentActivity.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = CurrentFragmentManager;
            }

            if (viewPager?.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
            {
                var ft = fragmentManager.BeginTransaction();
                var fragmentInfo = adapter.FragmentsInfo.Find(x => x.FragmentType == attribute.ViewType && x.ViewModelType == attribute.ViewModelType);
                var fragment = fragmentManager.FindFragmentByTag(fragmentInfo.Tag);
                adapter.FragmentsInfo.Remove(fragmentInfo);
                ft.Remove(fragment);
                ft.CommitAllowingStateLoss();
                adapter.NotifyDataSetChanged();

                OnFragmentPopped(ft, fragment, attribute);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        protected override bool CloseFragments()
        {
            try
            {
                CurrentFragmentManager?.PopBackStackImmediate();
            }
            catch (Exception ex)
            {
                MvxAndroidLog.Instance.Trace("Cannot close any fragments", ex);
            }
            return true;
        }

        protected override Task<bool> CloseFragment(IMvxViewModel viewModel, MvxFragmentPresentationAttribute attribute)
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
                fragmentManager.PopBackStackImmediate(fragmentName, 1);
                OnFragmentPopped(null, null, fragmentAttribute);

                return true;
            }
            else if (fragmentManager.Fragments.Count > 0 && fragmentManager.FindFragmentByTag(fragmentName) != null)
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

        protected override IMvxFragmentView CreateFragment(MvxBasePresentationAttribute attribute,
            string fragmentName)
        {
            try
            {
                var fragment = (IMvxFragmentView)Fragment.Instantiate(CurrentActivity, fragmentName);
                return fragment;
            }
            catch (Exception ex)
            {
                throw new MvxException(ex, $"Cannot create Fragment '{fragmentName}'. Are you use the wrong base class?");
            }
        }

        protected virtual new Fragment GetFragmentByViewType(Type type)
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
            if (!(fragManager?.Fragments?.Any() ?? false)) return null;

            foreach (var fragment in fragManager.Fragments)
            {
                //let's try again finding it
                var frag = fragment?.ChildFragmentManager?.FindFragmentByTag(fragmentName);

                //if we found the frag lets return it!
                if (frag != null)
                {
                    return frag;
                }

                //reloop for other fragments
                FindFragmentInChildren(fragmentName, fragment?.ChildFragmentManager);
            }

            return null;
        }
    }
}
