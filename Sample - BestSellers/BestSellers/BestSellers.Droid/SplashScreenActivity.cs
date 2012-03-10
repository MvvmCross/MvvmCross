using Android.App;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Android.Views;

namespace BestSellers.Droid
{
    [Activity(Label = "Bestseller", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity 
        : MvxBaseSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override MvxBaseAndroidSetup CreateSetup()
        {
            return new Setup(ApplicationContext);
        }
    }
}

