﻿
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Views;
using Xamarin.Forms;

namespace PageRendererExample.UI.Droid
{
    [Activity(Label = "PagerenderExample.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : MvxSplashScreenActivity
    {
        private bool isInitializationComplete = false;
        public override void InitializationComplete()
        {
            if (!isInitializationComplete)
            {
                isInitializationComplete = true;
                StartActivity(typeof(PageRendererApplicationActivity));
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            Forms.Init(this, bundle);

            base.OnCreate(bundle);
        }
    }
}