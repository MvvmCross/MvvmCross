using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Support.V4.App;
using Android.Support.V7.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public class MvxAppCompatViewPresenter : MvxAndroidViewPresenter
    {
        public MvxAppCompatViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        protected new AppCompatActivity CurrentActivity => (AppCompatActivity)base.CurrentActivity;
        protected new FragmentManager CurrentFragmentManager => CurrentActivity.SupportFragmentManager;

        protected override void ShowDialogFragment(Type view,
           MvxDialogAttribute attribute,
           MvxViewModelRequest request)
        {
            var fragmentName = FragmentJavaName(attribute.ViewType);
            var dialog = (DialogFragment)CreateFragment(fragmentName);
            dialog.Show(CurrentFragmentManager, fragmentName);
        }

        protected override IMvxFragmentView CreateFragment(string fragmentName)
        {
            var fragment = Fragment.Instantiate(CurrentActivity, fragmentName);
            return (IMvxFragmentView)fragment;
        }

        protected override void ShowFragment(Type view,
            MvxFragmentAttribute attribute,
            MvxViewModelRequest request)
        {
            ShowHostActivity(attribute);

            var fragmentName = FragmentJavaName(attribute.ViewType);
            var fragment = CreateFragment(fragmentName);

            var ft = CurrentFragmentManager.BeginTransaction();
            ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
            ft.CommitNowAllowingStateLoss();
        }
    }
}
