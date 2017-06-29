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

    public class MyViewPresenter : MvxAndroidPresenter
    {
        public MyViewPresenter(IEnumerable<Assembly> AndroidViewAssemblies) : base(AndroidViewAssemblies)
        {
        }

        protected override void ShowDialogFragment(MvxDialogAttribute attribute, MvxViewModelRequest request)
        {
            var activity = CurrentActivity as AppCompatActivity;

            var dialog = Fragment.Instantiate(activity, FragmentJavaName(attribute.ViewType)) as DialogFragment;
            dialog.Show(activity.SupportFragmentManager, attribute.ViewType.Name);
        }

        protected override void ShowFragment(MvxFragmentAttribute attribute, MvxViewModelRequest request)
        {
            var activity = CurrentActivity as AppCompatActivity;

            var fragmentName = FragmentJavaName(attribute.ViewType);
            var fragment = Fragment.Instantiate(CurrentActivity, fragmentName);

            var ft = activity.SupportFragmentManager.BeginTransaction();
            ft.Replace(attribute.FragmentContentId, fragment, fragmentName);
            ft.CommitNowAllowingStateLoss();
        }
    }
}