using Android.App;
using Android.Content.PM;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;

namespace $rootnamespace$
{
    [Activity(
        Label = "$rootnamespace$"
        , MainLauncher = true
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxStartActivity
    {
        public SplashScreen()
             : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
