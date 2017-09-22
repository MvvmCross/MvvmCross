using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace $rootnamespace$
{
    [Activity(
        Label = "YourAppName"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
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
            StartActivity(typeof(FormsActivity));
            base.TriggerFirstNavigate();
        }
    }
}
