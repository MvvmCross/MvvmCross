using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V7.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using RoutingExample.Core;
using System;
using System.Threading.Tasks;

namespace RoutingExample.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MyViewPresenter(AndroidViewAssemblies);
        }
    }

    public class MyViewPresenter : MvxAndroidViewPresenter
    {
        public MyViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        protected new AppCompatActivity CurrentActivity => (AppCompatActivity)base.CurrentActivity;
        protected new FragmentManager CurrentFragmentManager => CurrentActivity.SupportFragmentManager;

        protected override void ShowDialogFragment(Type view, 
           MvxDialogAttribute attribute, 
           MvxViewModelRequest request)
        {
            var dialog = Fragment.Instantiate(CurrentActivity, FragmentJavaName(attribute.ViewType)) as DialogFragment;
            dialog.Show(CurrentFragmentManager, attribute.ViewType.Name);
        }

        protected override IMvxFragmentView CreateFragment(string fragmentName)
        {
            var fragment = Fragment.Instantiate(CurrentActivity, fragmentName);
            return fragment as IMvxFragmentView;
        }

        protected override void ShowFragment(Type view,
            MvxFragmentAttribute attribute,
            MvxViewModelRequest request)
        {
            ShowHostActivity(attribute);

            var fragmentName = FragmentJavaName(attribute.ViewType);
            var fragment = CreateFragment(fragmentName);

            var ft = CurrentFragmentManager.BeginTransaction();
            ft.Replace(attribute.FragmentContentId, fragment as Fragment, fragmentName);
            ft.CommitNowAllowingStateLoss();
        }
    }
}