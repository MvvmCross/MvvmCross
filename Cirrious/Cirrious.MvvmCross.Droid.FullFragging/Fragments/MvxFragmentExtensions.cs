// MvxFragmentExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.OS;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.FullFragging.Caching;
using Cirrious.MvvmCross.Droid.FullFragging.Fragments.EventSource;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.FullFragging.Fragments
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

        public static void OnCreate(this IMvxFragmentView fragmentView, IMvxBundle bundle,
            MvxViewModelRequest request = null)
        {
            if (fragmentView.ViewModel != null)
            {
                Mvx.Trace("Fragment {0} already has a ViewModel, skipping ViewModel rehydration",
                    fragmentView.GetType().ToString());
                return;
            }

            var view = fragmentView as IMvxView;
            var viewModelType = fragmentView.FindAssociatedViewModelType();

            var cache = Mvx.Resolve<IMvxMultipleViewModelCache>();
            var cached = cache.GetAndClear(viewModelType, fragmentView.UniqueImmutableCacheTag);

            view.OnViewCreate(() => cached ?? LoadViewModel(fragmentView, bundle, request));
        }

        public static Type FindAssociatedViewModelType(this IMvxFragmentView fragmentView)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelTypeOrNull();

            var type = fragmentView.GetType();

            if (viewModelType == null)
            {
                if (!type.HasMvxFragmentAttribute())
                    throw new InvalidOperationException($"Your fragment is not generic and it does not have {nameof(MvxFragmentAttribute)} attribute set!");

                var cacheableFragmentAttribute = type.GetMvxFragmentAttribute();
                if (cacheableFragmentAttribute.ViewModelType == null)
                    throw new InvalidOperationException($"Your fragment is not generic and it does not use {nameof(MvxFragmentAttribute)} with ViewModel Type constructor.");

                viewModelType = cacheableFragmentAttribute.ViewModelType;
            }

            return viewModelType;
        }

        public static Fragment ToFragment(this IMvxFragmentView fragmentView)
        {
            var activity = fragmentView as Fragment;
            if (activity == null)
                throw new MvxException("ToFragment called on an IMvxFragmentView which is not an Android Fragment: {0}", fragmentView);
            return activity;
        }

        private static IMvxViewModel LoadViewModel(this IMvxFragmentView fragmentView, IMvxBundle savedState,
            MvxViewModelRequest request = null)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelType();
            if (viewModelType == typeof(MvxNullViewModel))
                return new MvxNullViewModel();

            if (viewModelType == null
                || viewModelType == typeof(IMvxViewModel))
            {
                MvxTrace.Trace("No ViewModel class specified for {0} in LoadViewModel",
                               fragmentView.GetType().Name);
            }

            if (request == null)
                request = MvxViewModelRequest.GetDefaultRequest(viewModelType);

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(request, savedState);

            return viewModel;
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment, LayoutInflater inflater)
        {
            var actualFragment = (Fragment)fragment;

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
            var actualFragment = (Fragment)fragment;

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
            var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(fragment.Activity.ApplicationContext);
            setupSingleton.EnsureInitialized();
        }

        public static void RegisterFragmentViewToCacheIfNeeded(this IMvxFragmentView fragmentView)
        {
            Fragment representedFragment = fragmentView as Fragment;

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
            fragmentCacheConfiguration.RegisterFragmentToCache(fragmentView.UniqueImmutableCacheTag, fragmentView.GetType(), fragmentView.FindAssociatedViewModelType());
        }
    }
}