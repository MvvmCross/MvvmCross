// MvxFragmentExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.OS;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Shared.Fragments.EventSource;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Support.V4.EventSource;

namespace MvvmCross.Droid.Support.V4
{
    public static class MvxFragmentExtensions
    {
        public static void AddEventListeners(this IMvxEventSourceFragment fragment)
        {
            if (fragment is IMvxFragmentView)
            {
                var adapter = new MvxBindingFragmentAdapter(fragment);
            }
        }

        public static void OnCreate(this IMvxFragmentView fragmentView, IMvxBundle bundle, MvxViewModelRequest request = null)
        {
            if (fragmentView.ViewModel != null)
            {
                //TODO call MvxViewModelLoader.Reload when it's added in MvvmCross, tracked by #1165
                //until then, we're going to re-run the viewmodel lifecycle here.
                MvxSharedFragmentExtensions.RunViewModelLifecycle(fragmentView.ViewModel, bundle, request);

                return;
            }

            Android.Support.V4.App.Fragment fragment = fragmentView.ToFragment();
            if (fragmentView == null)
                throw new InvalidOperationException($"Something really weird. ${nameof(fragmentView)} passed is not a Fragment!");

            // as it is called during onCreate it is safe to assume that fragment has Activity attached.
            var viewModelType = fragmentView.FindAssociatedViewModelType(fragment.Activity.GetType());
            var view = fragmentView as IMvxView;

            var cache = Mvx.Resolve<IMvxMultipleViewModelCache>();
            var cached = cache.GetAndClear(viewModelType, fragmentView.UniqueImmutableCacheTag);

            view.OnViewCreate(() => cached ?? fragmentView.LoadViewModel(bundle, fragment.Activity.GetType(), request));

        }

        private static Android.Support.V4.App.Fragment ToFragment(this IMvxFragmentView fragmentView)
        {
            return fragmentView as Android.Support.V4.App.Fragment;
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment, LayoutInflater inflater)
        {
            var actualFragment = fragment.ToFragment();

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new MvxAndroidBindingContext(actualFragment.Activity,
                    new MvxSimpleLayoutInflaterHolder(inflater),
                    fragment.DataContext);
            }
            else
            {
                var androidContext = fragment.BindingContext as IMvxAndroidBindingContext;
                if (androidContext != null)
                    androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(inflater);
            }
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment, Bundle b0)
        {
            var actualFragment = fragment.ToFragment();

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new MvxAndroidBindingContext(actualFragment.Activity,
                    new MvxSimpleLayoutInflaterHolder(
                        actualFragment.Activity.LayoutInflater),
                    fragment.DataContext);
            }
            else
            {
                var androidContext = fragment.BindingContext as IMvxAndroidBindingContext;
                if (androidContext != null)
                    androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(actualFragment.Activity.LayoutInflater);
            }
        }

        public static void EnsureSetupInitialized(this IMvxFragmentView fragmentView)
        {
            var fragment = fragmentView.ToFragment();
            if (fragment == null)
                throw new MvxException("EnsureSetupInitialized called on an IMvxFragmentView which is not an Android Fragment: {0}", fragmentView);

            var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(fragment.Activity.ApplicationContext);
            setupSingleton.EnsureInitialized();
        }

        public static void RegisterFragmentViewToCacheIfNeeded(this IMvxFragmentView fragmentView, Type fragmentParentActivityType)
        {
            Android.Support.V4.App.Fragment representedFragment = fragmentView.ToFragment();

            if (representedFragment == null)
                throw new InvalidOperationException($"Represented type: {fragmentView.GetType()} is not a Fragment!");

            var fragmentParentActivtiy = representedFragment.Activity;

            if (fragmentParentActivtiy == null)
                throw new InvalidOperationException("Something wrong happend, fragment has no activity attached during registration!");

            IFragmentCacheableActivity cacheableActivity = fragmentParentActivtiy as IFragmentCacheableActivity;

            if (cacheableActivity == null)
                throw new InvalidOperationException($"Fragment has activity attached but it does not implement {nameof(IFragmentCacheableActivity)} ! Cannot register fragment to cache!");

            if (string.IsNullOrEmpty(fragmentView.UniqueImmutableCacheTag))
                throw new InvalidOperationException("Contract failed - Fragment tag is null! Fragment tags are not set by default, you should add tag during FragmentTransaction or override UniqueImmutableCacheTag in your Fragment class.");

            var fragmentCacheConfiguration = cacheableActivity.FragmentCacheConfiguration;
            fragmentCacheConfiguration.RegisterFragmentToCache(fragmentView.UniqueImmutableCacheTag, fragmentView.GetType(), fragmentView.FindAssociatedViewModelType(fragmentParentActivityType));
        }
    }
}