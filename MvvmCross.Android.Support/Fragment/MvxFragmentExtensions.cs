﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Support.V4.App;
using Android.Views;
using MvvmCross.Core;
using MvvmCross.Droid.Support.V4.EventSource;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;

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
                Platforms.Android.Views.MvxFragmentExtensions.RunViewModelLifecycle(fragmentView.ViewModel, bundle, request);

                return;
            }

            var fragment = fragmentView.ToFragment();
            if (fragment == null)
                throw new MvxException($"{nameof(OnCreate)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

            // as it is called during onCreate it is safe to assume that fragment has Activity attached.
            var viewModelType = fragmentView.FindAssociatedViewModelType(fragment.Activity.GetType());
            var view = fragmentView as IMvxView;

            var cache = Mvx.Resolve<IMvxMultipleViewModelCache>();
            var cached = cache.GetAndClear(viewModelType, fragmentView.UniqueImmutableCacheTag);

            view.OnViewCreate(() => cached ?? fragmentView.LoadViewModel(bundle, fragment.Activity.GetType(), request));
        }

        public static Fragment ToFragment(this IMvxFragmentView fragmentView)
        {
            return fragmentView as Fragment;
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment, LayoutInflater inflater)
        {
            var actualFragment = fragment.ToFragment();
            if (actualFragment == null)
                throw new MvxException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragment}");

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

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment)
        {
            var actualFragment = fragment.ToFragment();
            if (actualFragment == null)
                throw new MvxException($"{nameof(EnsureBindingContextIsSet)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragment}");

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
                throw new MvxException($"{nameof(EnsureSetupInitialized)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(fragment.Activity.ApplicationContext);
            setup.EnsureInitialized();
        }

        public static TFragment FindFragmentById<TFragment>(this MvxFragmentActivity activity, int resourceId)
            where TFragment : Fragment
        {
            var fragment = activity.SupportFragmentManager.FindFragmentById(resourceId);
            if (fragment == null) {
                MvxAndroidLog.Instance.Warn("Failed to find fragment id {0} in {1}", resourceId, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        public static TFragment FindFragmentByTag<TFragment>(this MvxFragmentActivity activity, string tag)
            where TFragment : Fragment
        {
            var fragment = activity.SupportFragmentManager.FindFragmentByTag(tag);
            if (fragment == null) {
                MvxAndroidLog.Instance.Warn("Failed to find fragment tag {0} in {1}", tag, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        private static TFragment SafeCast<TFragment>(Fragment fragment) where TFragment : Fragment
        {
            if (!(fragment is TFragment)) {
                MvxAndroidLog.Instance.Warn("Fragment type mismatch got {0} but expected {1}", fragment.GetType().FullName,
                            typeof(TFragment).FullName);
                return default(TFragment);
            }

            return (TFragment)fragment;
        }

        public static void LoadViewModelFrom(this IMvxFragmentView view, MvxViewModelRequest request, IMvxBundle savedState = null)
        {
            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(request, savedState);
            if (viewModel == null) {
                MvxAndroidLog.Instance.Warn("ViewModel not loaded for {0}", request.ViewModelType.FullName);
                return;
            }

            view.ViewModel = viewModel;
        }
    }
}
