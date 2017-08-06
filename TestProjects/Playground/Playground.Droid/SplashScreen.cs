using System;
using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace Playground.Droid
{
    [Activity(
        Label = "Playground.Droid"
        , MainLauncher = true
        , Icon = "@mipmap/icon"
        , Theme = "@style/AppTheme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
