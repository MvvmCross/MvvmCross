// MvxFragmentExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Platform;

namespace MvvmCross.Droid.FullFragging
{
    public static class MvxFragmentExtensionMethods
    {
        public static TFragment FindFragmentById<TFragment>(this Views.MvxActivity activity, int resourceId)
            where TFragment : Fragment
        {
            var fragment = activity.FragmentManager.FindFragmentById(resourceId);
            if (fragment == null)
            {
                Mvx.Warning("Failed to find fragment id {0} in {1}", resourceId, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        public static TFragment FindFragmentByTag<TFragment>(this Views.MvxActivity activity, string tag)
            where TFragment : Fragment
        {
            var fragment = activity.FragmentManager.FindFragmentByTag(tag);
            if (fragment == null)
            {
                Mvx.Warning("Failed to find fragment tag {0} in {1}", tag, activity.GetType().Name);
                return default(TFragment);
            }

            return SafeCast<TFragment>(fragment);
        }

        private static TFragment SafeCast<TFragment>(Fragment fragment) where TFragment : Fragment
        {
            if (!(fragment is TFragment))
            {
                Mvx.Warning("Fragment type mismatch got {0} but expected {1}", fragment.GetType().FullName,
                            typeof(TFragment).FullName);
                return default(TFragment);
            }

            return (TFragment)fragment;
        }

        public static void LoadViewModelFrom(this IMvxFragmentView view, MvxViewModelRequest request, IMvxBundle savedState = null)
        {
            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(request, savedState);
            if (viewModel == null)
            {
                Mvx.Warning("ViewModel not loaded for {0}", request.ViewModelType.FullName);
                return;
            }

            view.ViewModel = viewModel;
        }
    }
}