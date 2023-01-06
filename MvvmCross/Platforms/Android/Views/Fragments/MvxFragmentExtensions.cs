// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace MvvmCross.Platforms.Android.Views.Fragments
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
            var cache = Mvx.IoCProvider.Resolve<IMvxMultipleViewModelCache>();

            if (fragmentView.ViewModel != null)
            {
                // check if ViewModel instance was cached. If so, clear it and ignore previous instance
                cache.GetAndClear(fragmentView.ViewModel.GetType(), fragmentView.UniqueImmutableCacheTag);
                return;
            }

            var fragment = fragmentView.ToFragment();
            if (fragment == null)
                throw new MvxException($"{nameof(OnCreate)} called on an {nameof(IMvxFragmentView)} which is not an Android Fragment: {fragmentView}");

            // as it is called during onCreate it is safe to assume that fragment has Activity attached.
            var viewModelType = fragmentView.FindAssociatedViewModelType(fragment.Activity.GetType());
            var view = fragmentView as IMvxView;

            var cached = cache.GetAndClear(viewModelType, fragmentView.UniqueImmutableCacheTag);

            view.OnViewCreate(() => cached ?? fragmentView.LoadViewModel(bundle, fragment.Activity.GetType(), request));
        }

        public static Fragment? ToFragment(this IMvxFragmentView fragmentView)
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
            else if (fragment.BindingContext is IMvxAndroidBindingContext androidContext)
            {
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
            else if (fragment.BindingContext is IMvxAndroidBindingContext androidContext)
            {
                androidContext.LayoutInflaterHolder = new MvxSimpleLayoutInflaterHolder(actualFragment.Activity.LayoutInflater);
            }
        }

        public static TFragment FindFragmentById<TFragment>(this MvxActivity activity, int resourceId)
            where TFragment : Fragment
        {
            var fragment = activity.SupportFragmentManager.FindFragmentById(resourceId);
            if (fragment == null)
            {
                MvxLogHost.Default?.Log(LogLevel.Warning,
                    "Failed to find fragment id {ResourceId} in {ActivityTypeName}", resourceId, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        public static TFragment FindFragmentByTag<TFragment>(this MvxActivity activity, string tag)
            where TFragment : Fragment
        {
            var fragment = activity.SupportFragmentManager.FindFragmentByTag(tag);
            if (fragment == null)
            {
                MvxLogHost.Default?.Log(LogLevel.Warning,
                    "Failed to find fragment tag {Tag} in {ActivityTypeName}", tag, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        private static TFragment SafeCast<TFragment>(Fragment fragment) where TFragment : Fragment
        {
            if (!(fragment is TFragment))
            {
                MvxLogHost.Default?.Log(LogLevel.Warning,
                    "Fragment type mismatch got {FragmentType} but expected {ExpectedType}",
                    fragment.GetType().FullName, typeof(TFragment).FullName);
                return default(TFragment);
            }

            return (TFragment)fragment;
        }

        public static void LoadViewModelFrom(this Android.Views.IMvxFragmentView view, MvxViewModelRequest request, IMvxBundle savedState = null)
        {
            var loader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(request, savedState);
            if (viewModel == null)
            {
                MvxLogHost.Default?.Log(LogLevel.Warning, "ViewModel not loaded for {ViewModelType}", request.ViewModelType.FullName);
                return;
            }

            view.ViewModel = viewModel;
        }
    }
}
