// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Droid.Views.Fragments
{
    public static class MvxFragmentExtensionMethods
    {
        public static TFragment FindFragmentById<TFragment>(this MvxActivity activity, int resourceId)
            where TFragment : Fragment
        {
            var fragment = activity.FragmentManager.FindFragmentById(resourceId);
            if (fragment == null)
            {
                MvxLog.Instance.Warn("Failed to find fragment id {0} in {1}", resourceId, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        public static TFragment FindFragmentByTag<TFragment>(this MvxActivity activity, string tag)
            where TFragment : Fragment
        {
            var fragment = activity.FragmentManager.FindFragmentByTag(tag);
            if (fragment == null)
            {
                MvxLog.Instance.Warn("Failed to find fragment tag {0} in {1}", tag, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        private static TFragment SafeCast<TFragment>(Fragment fragment) where TFragment : Fragment
        {
            if (!(fragment is TFragment))
            {
                MvxLog.Instance.Warn("Fragment type mismatch got {0} but expected {1}", fragment.GetType().FullName,
                            typeof(TFragment).FullName);
                return default(TFragment);
            }

            return (TFragment)fragment;
        }

        public static void LoadViewModelFrom(this Droid.Views.IMvxFragmentView view, MvxViewModelRequest request, IMvxBundle savedState = null)
        {
            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(request, savedState);
            if (viewModel == null)
            {
                MvxLog.Instance.Warn("ViewModel not loaded for {0}", request.ViewModelType.FullName);
                return;
            }

            view.ViewModel = viewModel;
        }
    }
}
