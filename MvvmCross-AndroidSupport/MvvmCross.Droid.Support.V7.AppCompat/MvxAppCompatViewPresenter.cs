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
               typeof(MvxTabAttribute),
               (view, attribute, request) => ShowTab(view, (MvxTabAttribute)attribute, request));
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
                    foreach (var item in attributes.OfType<MvxFragmentAttribute>())
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
            if (viewType.GetInterfaces().Contains(typeof(Android.Content.IDialogInterface)))
                return new MvxDialogAttribute();
            if (viewType.IsSubclassOf(typeof(Fragment)))
                return new MvxFragmentAttribute(GetCurrentActivityViewModelType());

            return new MvxActivityAttribute() { ViewModelType = viewModelType };
        }

        protected override void ShowActivity(Type view, 
            MvxActivityAttribute attribute, 
            MvxViewModelRequest request)
        {
            var intent = CreateIntentForRequest(request);
            if(attribute.Extras != null)
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

        protected override void ShowHostActivity(MvxFragmentAttribute attribute)
        {
            var viewType = ViewsContainer.GetViewType(attribute.ActivityHostViewModelType);
            if (!viewType.IsSubclassOf(typeof(FragmentActivity)))
                throw new MvxException("The host activity doesnt inherit FragmentActivity");

            var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            Show(hostViewModelRequest);
        }

        protected override void ShowFragment(Type view,
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

                if(attribute.AddToBackStack == true)
                    ft.AddToBackStack(fragmentName);

                ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
                ft.CommitAllowingStateLoss();
            }
        }

        protected override void ShowDialogFragment(Type view,
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

        protected virtual void ShowTab(
            Type view,
            MvxTabAttribute attribute,
            MvxViewModelRequest request)
        {
            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                _pendingRequest = request;
                //TODO: Fix host activity
                //ShowHostActivity(attribute);
            }
            else
            {
                if (CurrentActivity.FindViewById(attribute.TabLayoutResourceId) == null)
                    throw new NullReferenceException("FrameLayout to show Fragment not found");

                var viewPager = CurrentActivity.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                var tabLayout = CurrentActivity.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
                if (viewPager != null && tabLayout != null)
                {
                    if (viewPager.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
                    {
                        if(adapter.Fragments.Any(f => f.Tag == attribute.Title))
                        {
                            var index = adapter.Fragments.FindIndex(f => f.Tag == attribute.Title);
                            viewPager.CurrentItem = index > -1 ? index : 0;
                        }
                        else
                        {
                            if(request is MvxViewModelInstanceRequest instanceRequest)
                                adapter.Fragments.Add(new MvxViewPagerFragment(attribute.Title, attribute.ViewType, instanceRequest.ViewModelInstance));
                            else
                                adapter.Fragments.Add(new MvxViewPagerFragment(attribute.Title, attribute.ViewType, attribute.ViewModelType));
                            adapter.NotifyDataSetChanged();
                        }
                    }
                    else
                    {
                        var fragments = new List<MvxViewPagerFragment>();
                        if (request is MvxViewModelInstanceRequest instanceRequest)
                            fragments.Add(new MvxViewPagerFragment(attribute.Title, attribute.ViewType, instanceRequest.ViewModelInstance));
                        else
                            fragments.Add(new MvxViewPagerFragment(attribute.Title, attribute.ViewType, attribute.ViewModelType));

                        if(attribute.FragmentHostViewType != null)
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
                    tabLayout.SetupWithViewPager(viewPager);
                }
                else
                    throw new MvxException("ViewPager or TabLayout not found");
            }
        }

        public override void Close(IMvxViewModel viewModel)
        {
            var attribute = GetAttributeForViewModel(viewModel.GetType());

            if (attribute is MvxActivityAttribute)
            {
                //TODO: Find something to close the dialogs

                if (CurrentFragmentManager.BackStackEntryCount > 0)
                    CurrentFragmentManager.PopBackStackImmediate(null, 0);

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
            else if (attribute is MvxFragmentAttribute fragmentAttribute)
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
            else if (attribute is MvxDialogAttribute dialogAttribute)
            {
                if (Dialogs.TryGetValue(viewModel, out DialogFragment dialog))
                {
                    dialog.DismissAllowingStateLoss();
                    dialog.Dispose();
                    Dialogs.Remove(viewModel);
                }
            }
        }

        protected override IMvxFragmentView CreateFragment(MvxBasePresentationAttribute attribute,
            string fragmentName)
        {
            try
            {
                IMvxFragmentView fragment;
                if (attribute is MvxFragmentAttribute fragmentAttribute && fragmentAttribute.IsCacheableFragment)
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
