using Android.App;
using Cirrious.MvvmCross.Droid.Views;

namespace $rootnamespace$
{
    [Activity(Label = "$rootnamespace$", MainLauncher = true, Icon = "@drawable/icon")]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}