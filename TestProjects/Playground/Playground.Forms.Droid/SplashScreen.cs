using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace Playground.Forms.Droid
{
    // No Splash Screen: To remove splash screen, remove this class and uncomment lines in MainActivity
    [Activity(
        Label = "Playground.Forms"
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

        protected override void TriggerFirstNavigate()
        {
            StartActivity(typeof(MainActivity));
            base.TriggerFirstNavigate();
        }
    }
}
