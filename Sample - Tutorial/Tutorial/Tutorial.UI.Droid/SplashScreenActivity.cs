
using Android.App;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Android.Views;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;

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

        protected override void OnViewModelSet()
        {
            // ignored
        }
    }
}

