
using Android.App;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Android.Views;

namespace CustomerManagement.Droid
{
    [Activity(Label = "SplashScreenActivity", Theme = "@android:style/Theme.Black.NoTitleBar", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class SplashScreenActivity
        : MvxBaseSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.Splash)
        {
        }
    }
}