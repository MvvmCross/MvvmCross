
using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;

namespace Tutorial.UI.Droid
{
    [Activity(Label = "Tutorial.UI.Droid", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
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

