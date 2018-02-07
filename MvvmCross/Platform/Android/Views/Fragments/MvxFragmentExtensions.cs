﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Droid.Views.Fragments
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
                Droid.Views.MvxFragmentExtensions.RunViewModelLifecycle(fragmentView.ViewModel, bundle, request);

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

        public async static Task EnsureSetupInitialized(this IMvxFragmentView fragmentView)
        {
            var fragment = fragmentView.ToFragment();
            if (fragment == null)
                throw new MvxException($"{nameof(EnsureSetupInitialized)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

            var setupSingleton = await MvxAndroidSetupSingleton.EnsureSingletonAvailable(fragment.Activity.ApplicationContext);
            await setupSingleton.EnsureInitialized();
        }
    }
}
