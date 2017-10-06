using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Util;
using Android.Support.V4.View;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public class MvxAppCompatViewPresenter : MvxAndroidViewPresenter
    {
        public MvxAppCompatViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {

        }

        protected new ConditionalWeakTable<IMvxViewModel, DialogFragment> Dialogs { get; } = new ConditionalWeakTable<IMvxViewModel, DialogFragment>();

        protected new FragmentManager CurrentFragmentManager
        {
            get
            {
                if (CurrentActivity is FragmentActivity activity)
                    return activity.SupportFragmentManager;
                throw new InvalidCastException("Cannot use Android Support Fragment within non AppCompat Activity");
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

        public override MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType)
        {
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

                return GetOverridePresentationAttribute(attribute.ViewType) ?? attribute;
            }

            return CreateAttributeForViewModel(viewModelType);
        }

        public override MvxBasePresentationAttribute CreateAttributeForViewModel(Type viewModelType)
        {
            var viewType = ViewsContainer.GetViewType(viewModelType);
            if (viewType.IsSubclassOf(typeof(DialogFragment)))
            {
                MvxTrace.Trace("PresentationAttribute not found for {0}. Assuming DialogFragment presentation", viewModelType.Name);
                return new MvxDialogFragmentPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            }
            if (viewType.IsSubclassOf(typeof(Fragment)))
            {
                MvxTrace.Trace("PresentationAttribute not found for {0}. Assuming Fragment presentation", viewModelType.Name);
                return new MvxFragmentPresentationAttribute(GetCurrentActivityViewModelType(), Android.Resource.Id.Content) { ViewType = viewType, ViewModelType = viewModelType };
            }

            return base.CreateAttributeForViewModel(viewModelType);
        }

        #region Show implementations
        protected override void ShowActivity(Type view,
            MvxActivityPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var intent = CreateIntentForRequest(request);
            if (attribute.Extras != null)
                intent.PutExtras(attribute.Extras);

            var activity = CurrentActivity;
            if (activity == null)
            {
                MvxTrace.Warning("Cannot Resolve current top activity");
                return;
            }

            var activityOptions = CreateActivityTransitionOptions(intent, attribute);
            if (activityOptions != null)
            {
                activity.StartActivity(intent, activityOptions.ToBundle());
            }
            else
                activity.StartActivity(intent);
        }

        protected virtual ActivityOptionsCompat CreateActivityTransitionOptions(Android.Content.Intent intent,MvxActivityPresentationAttribute attribute){
            ActivityOptionsCompat options = null;
            if (attribute.SharedElements != null)
			{
				IList<Pair> sharedElements = new List<Pair>();
				foreach (var item in attribute.SharedElements)
				{
					intent.PutExtra(item.Key, ViewCompat.GetTransitionName(item.Value));
					sharedElements.Add(Pair.Create(item.Value, item.Key));
				}
				options = ActivityOptionsCompat.MakeSceneTransitionAnimation(CurrentActivity, sharedElements.ToArray());
			}

            return options;
        }

        protected override void ShowHostActivity(MvxFragmentPresentationAttribute attribute)
        {
            var viewType = ViewsContainer.GetViewType(attribute.ActivityHostViewModelType);
            if (!viewType.IsSubclassOf(typeof(FragmentActivity)))
                throw new MvxException("The host activity doesnt inherit FragmentActivity");

            var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            Show(hostViewModelRequest);
        }

        protected override void ShowFragment(Type view,
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

                PerformShowFragmentTransaction(CurrentFragmentManager, attribute, request);
            }
        }

        protected override void ShowNestedFragment(Type view, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
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

            OnBeforeFragmentChanging(ft, fragmentView, attribute);

            if (attribute.AddToBackStack == true)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, fragmentView, attribute);

            ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
            ft.CommitAllowingStateLoss();

            OnFragmentChanged(ft, fragmentView, attribute);
        }

        protected virtual void OnBeforeFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute){
			if (attribute.SharedElements != null)
			{
				foreach (var item in attribute.SharedElements)
				{
					string name = item.Key;
					if (string.IsNullOrEmpty(name))
						name = ViewCompat.GetTransitionName(item.Value);
					ft.AddSharedElement(item.Value, name);
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

        protected virtual void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute){
            
        }

		protected virtual void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
		{

		}

        protected virtual void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
		{

		}

        protected override void ShowDialogFragment(Type view,
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

            OnBeforeFragmentChanging(ft, dialog, attribute);

			if (attribute.AddToBackStack == true)
				ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, dialog, attribute);

            dialog.Show(ft, fragmentName);

            OnFragmentChanged(ft, dialog, attribute);
        }

        protected virtual void ShowViewPagerFragment(
            Type view,
            MvxViewPagerFragmentPresentationAttribute attribute,
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
                var viewPager = CurrentActivity.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                if (viewPager != null)
                {
                    if (viewPager.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
                    {
                        if (adapter.FragmentsInfo.Any(f => f.Tag == attribute.Title))
                        {
                            var index = adapter.FragmentsInfo.FindIndex(f => f.Tag == attribute.Title);
                            viewPager.SetCurrentItem(index > -1 ? index : 0, true);
                        }
                        else
                        {
                            if (request is MvxViewModelInstanceRequest instanceRequest)
                                adapter.FragmentsInfo.Add(new MvxViewPagerFragmentInfo(attribute.Title, attribute.ViewType, instanceRequest.ViewModelInstance));
                            else
                                adapter.FragmentsInfo.Add(new MvxViewPagerFragmentInfo(attribute.Title, attribute.ViewType, attribute.ViewModelType));
                            adapter.NotifyDataSetChanged();
                        }
                    }
                    else
                    {
                        var fragments = new List<MvxViewPagerFragmentInfo>();
                        if (request is MvxViewModelInstanceRequest instanceRequest)
                            fragments.Add(new MvxViewPagerFragmentInfo(attribute.Title, attribute.ViewType, instanceRequest.ViewModelInstance));
                        else
                            fragments.Add(new MvxViewPagerFragmentInfo(attribute.Title, attribute.ViewType, attribute.ViewModelType));

                        if (attribute.FragmentHostViewType != null)
                        {
                            var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
                            if (fragment == null)
                                throw new MvxException("Fragment not found", attribute.FragmentHostViewType.Name);

                            viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(CurrentActivity, fragment.ChildFragmentManager, fragments);
                        }
                        else
                            viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(CurrentActivity, CurrentFragmentManager, fragments);
                    }
                }
                else
                    throw new MvxException("ViewPager not found");
            }
        }

        protected virtual void ShowTabLayout(
            Type view,
            MvxTabLayoutPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ShowViewPagerFragment(view, attribute, request);

            var viewPager = CurrentActivity.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
            var tabLayout = CurrentActivity.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            if (viewPager != null && tabLayout != null)
            {
                tabLayout.SetupWithViewPager(viewPager);
            }
            else
                throw new MvxException("ViewPager or TabLayout not found");
        }
        #endregion

        #region Close implementations
        protected override bool CloseFragmentDialog(IMvxViewModel viewModel, MvxDialogFragmentPresentationAttribute attribute)
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

        protected virtual bool CloseViewPagerFragment(IMvxViewModel viewModel, MvxViewPagerFragmentPresentationAttribute attribute)
        {
            var viewPager = CurrentActivity.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
            if (viewPager?.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
            {
                var ft = CurrentFragmentManager.BeginTransaction();
                var fragmentInfo = adapter.FragmentsInfo.Find(x => x.FragmentType == attribute.ViewType && x.ViewModelType == attribute.ViewModelType);
                var fragment = CurrentFragmentManager.FindFragmentByTag(fragmentInfo.Tag);
                adapter.FragmentsInfo.Remove(fragmentInfo);
                ft.Remove(fragment);
                ft.CommitAllowingStateLoss();
                adapter.NotifyDataSetChanged();

                OnFragmentPopped(ft, fragment, attribute);
                return true;
            }
            return false;
        }

        protected override bool CloseFragment(IMvxViewModel viewModel, MvxFragmentPresentationAttribute attribute)
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
                fragmentManager.PopBackStackImmediate(fragmentName, 1);

                OnFragmentPopped(null,null,fragmentAttribute);

                return true;
            }
            else if (fragmentManager.Fragments.Count > 0 && fragmentManager.FindFragmentByTag(fragmentAttribute.ViewType.Name) != null)
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

                OnFragmentPopped(ft, fragment ,fragmentAttribute);

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
            catch
            {
                throw new MvxException($"Cannot create Fragment '{fragmentName}'. Are you use the wrong base class?");
            }
        }

        protected virtual new Fragment GetFragmentByViewType(Type type)
        {
            var fragmentName = FragmentJavaName(type);
            return CurrentFragmentManager.FindFragmentByTag(fragmentName);
        }
    }
}
