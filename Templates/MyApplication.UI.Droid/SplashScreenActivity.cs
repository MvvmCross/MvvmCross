using Android.App;
using Cirrious.MvvmCross.Droid.Views;

namespace MyApplication.UI.Droid
{
    [Activity(Label = "MyApplication", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity
        : MvxBaseSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnViewModelSet()
        {
            // ignored
        }
    }
}

