using System;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class SplitRootView : MvxAppCompatActivity<SplitRootViewModel>
    {
        public DrawerLayout DrawerLayout { get; set; }

        public SplitRootView()
        {
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SplitRootView);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if (bundle == null)
            {
                ViewModel.ShowInitialMenuCommand.Execute();
                ViewModel.ShowDetailCommand.Execute();
            }
        }

        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
                DrawerLayout.CloseDrawers();
            else
                base.OnBackPressed();
        }
    }
}
