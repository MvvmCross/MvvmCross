using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace RoutingExample.Droid
{
    [Activity(
        Label = "RoutingExample.Droid"
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
