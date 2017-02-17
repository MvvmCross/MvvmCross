using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace MvvmCross.TestProjects.CustomBinding.Droid
{
    [Activity(
        Label = "MvvmCross.TestProjects.CustomBinding.Droid"
        , MainLauncher = true
        , Icon = "@mipmap/icon"
        , Theme = "@style/Theme.Splash"
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
