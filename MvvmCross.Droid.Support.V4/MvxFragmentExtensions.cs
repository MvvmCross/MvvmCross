// MvxFragmentExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using System;
using MvvmCross.Droid.Shared.Fragments.EventSource;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Shared.Attributes;
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
                RunViewModelLifecycle(fragmentView.ViewModel, bundle, request);

                return;
            }

            var viewModelType = fragmentView.FindAssociatedViewModelType();
            var view = fragmentView as IMvxView;

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

        private static void RunViewModelLifecycle(IMvxViewModel viewModel, IMvxBundle savedState,
            MvxViewModelRequest request)
        {
            try
            {
                if (request != null)
                {
                    var parameterValues = new MvxBundle(request.ParameterValues);
                    viewModel.CallBundleMethods("Init", parameterValues);
                }
                if (savedState != null)
                {
                    viewModel.CallBundleMethods("ReloadState", savedState);
                }
                viewModel.Start();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
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