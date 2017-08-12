using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Util;
using Android.Support.V4.View;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
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

        protected override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();
            AttributeTypesToShowMethodDictionary.Add(
               typeof(MvxTabLayoutPresentationAttribute),
               (view, attribute, request) => ShowTabLayout(view, (MvxTabLayoutPresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxViewPagerFragmentPresentationAttribute),
               (view, attribute, request) => ShowViewPagerFragment(view, (MvxViewPagerFragmentPresentationAttribute)attribute, request));
        }

        protected override MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType)
        {
            IList<MvxBasePresentationAttribute> attributes;
            if (ViewModelToPresentationAttributeMap.TryGetValue(viewModelType, out attributes))
            {
                MvxBasePresentationAttribute attribute = attributes.FirstOrDefault();

                if (attributes.Count > 1)
                {
                    var currentHostViewModelType = GetCurrentActivityViewModelType();
                    foreach (var item in attributes.OfType<MvxFragmentPresentationAttribute>())
                    {
                        if (CurrentActivity.FindViewById(item.FragmentContentId) != null && item.ActivityHostViewModelType == currentHostViewModelType)
                        {
                            attribute = item;
                            break;
                        }
                    }
                }

                if (attribute.ViewType?.GetInterfaces().OfType<IMvxOverridePresentationAttribute>().FirstOrDefault() is IMvxOverridePresentationAttribute view)
                {
                    var presentationAttribute = view.PresentationAttribute();

                    if (presentationAttribute != null)
                        return presentationAttribute;
                }
                return attribute;
            }

            var viewType = ViewsContainer.GetViewType(viewModelType);
            if (viewType.IsSubclassOf(typeof(DialogFragment)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                    $"Assuming DialogFragment presentation");
                return new MvxDialogFragmentPresentationAttribute();
            }
            if (viewType.IsSubclassOf(typeof(Fragment)))
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                    $"Assuming Fragment presentation");
                return new MvxFragmentPresentationAttribute(GetCurrentActivityViewModelType(), Android.Resource.Id.Content);
            }

            MvxTrace.Trace($"PresentationAttribute not found for {viewModelType.Name}. " +
                    $"Assuming Activity presentation");
            return new MvxActivityPresentationAttribute() { ViewModelType = viewModelType };
        }

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

            if (attribute.SharedElements != null)
            {
                IList<Pair> sharedElements = new List<Pair>();
                foreach (var item in attribute.SharedElements)
                {
                    intent.PutExtra(item.Key, ViewCompat.GetTransitionName(item.Value));
                    sharedElements.Add(Pair.Create(item.Value, item.Key));
                }
                ActivityOptionsCompat options = ActivityOptionsCompat.MakeSceneTransitionAnimation(CurrentActivity, sharedElements.ToArray());
                activity.StartActivity(intent, options.ToBundle());
            }
            else
                activity.StartActivity(intent);
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

                var ft = CurrentFragmentManager.BeginTransaction();
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

                if (attribute.AddToBackStack == true)
                    ft.AddToBackStack(fragmentName);

                ft.Add(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
                ft.CommitAllowingStateLoss();
            }
        }

        protected override void ShowDialogFragment(Type view,
           MvxDialogFragmentPresentationAttribute attribute,
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

            Dialogs.Add(viewModel, dialog);

            var ft = CurrentFragmentManager.BeginTransaction();
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

            if (attribute.AddToBackStack == true)
                ft.AddToBackStack(fragmentName);

            dialog.Show(ft, fragmentName);
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
                            var fragmentName = FragmentJavaName(attribute.FragmentHostViewType);
                            var fragment = CurrentFragmentManager.FindFragmentByTag(fragmentName);

                            if (fragment == null)
                                throw new MvxException("Fragment not found", fragmentName);
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

        public override void Close(IMvxViewModel viewModel)
        {
            var attribute = GetAttributeForViewModel(viewModel.GetType());

            if (attribute is MvxActivityPresentationAttribute)
            {
                //TODO: Find something to close the dialogs

                if (CurrentFragmentManager.BackStackEntryCount > 0)
                    CurrentFragmentManager.PopBackStackImmediate(null, 0);
                if (CurrentFragmentManager.Fragments.Count > 0)
                {
                    foreach (var fragment in CurrentFragmentManager.Fragments)
                    {
                        var ft = CurrentFragmentManager.BeginTransaction();
                        ft.Remove(fragment);
                        ft.CommitAllowingStateLoss();
                    }
                }

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
            else if (attribute is MvxDialogFragmentPresentationAttribute dialogAttribute)
            {
                if (Dialogs.TryGetValue(viewModel, out DialogFragment dialog))
                {
                    dialog.DismissAllowingStateLoss();
                    dialog.Dispose();
                    Dialogs.Remove(viewModel);
                }
            }
            else if (attribute is MvxViewPagerFragmentPresentationAttribute viewPagerAttribute)
            {
                var viewPager = CurrentActivity.FindViewById<ViewPager>(viewPagerAttribute.ViewPagerResourceId);
                if (viewPager != null)
                {
                    if (viewPager.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
                    {
                        var ft = CurrentFragmentManager.BeginTransaction();
                        var fragmentInfo = adapter.FragmentsInfo.Find(x => x.FragmentType == viewPagerAttribute.ViewType && x.ViewModelType == viewPagerAttribute.ViewModelType);
                        var fragment = CurrentFragmentManager.FindFragmentByTag(fragmentInfo.Tag);
                        adapter.FragmentsInfo.Remove(fragmentInfo);
                        ft.Remove(fragment);
                        ft.CommitAllowingStateLoss();
                        adapter.NotifyDataSetChanged();
                    }
                }
            }
            else if (attribute is MvxFragmentPresentationAttribute fragmentAttribute)
            {
                if (CurrentFragmentManager.BackStackEntryCount > 0)
                {
                    var fragmentName = FragmentJavaName(fragmentAttribute.ViewType);
                    CurrentFragmentManager.PopBackStackImmediate(fragmentName, 1);
                }
                else if (CurrentFragmentManager.Fragments.Count > 0 && CurrentFragmentManager.FindFragmentByTag(fragmentAttribute.ViewType.Name) != null)
                {
                    var ft = CurrentFragmentManager.BeginTransaction();
                    var fragment = CurrentFragmentManager.FindFragmentByTag(fragmentAttribute.ViewType.Name);

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
                }
                else
                    CurrentActivity.Finish();
            }
        }

        protected override IMvxFragmentView CreateFragment(MvxBasePresentationAttribute attribute,
            string fragmentName)
        {
            try
            {
                IMvxFragmentView fragment;
                if (attribute is MvxFragmentPresentationAttribute fragmentAttribute && fragmentAttribute.IsCacheableFragment)
                {
                    if (CachedFragments.TryGetValue(attribute, out fragment))
                        return fragment;

                    fragment = (IMvxFragmentView)Fragment.Instantiate(CurrentActivity, fragmentName);
                    CachedFragments.Add(attribute, fragment);
                }
                else
                    fragment = (IMvxFragmentView)Fragment.Instantiate(CurrentActivity, fragmentName);
                return fragment;
            }
            catch
            {
                throw new MvxException($"Cannot create Fragment '{fragmentName}'. Are you use the wrong base class?");
            }
        }
    }
}
