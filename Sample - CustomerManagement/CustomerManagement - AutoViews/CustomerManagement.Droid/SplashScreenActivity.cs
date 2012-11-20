using Android.App;
using Cirrious.MvvmCross.Droid.Views;

namespace CustomerManagement.AutoViews.Droid
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