using Android.App;
using Android.Graphics.Drawables;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Android.Views;

namespace Cirrious.Conference.UI.Droid
{
    [Activity(Label = "SqlBits", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity
        : MvxBaseSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);
            this.SetBackground();
        }

        protected override MvxBaseAndroidSetup CreateSetup()
        {
            return new Setup(ApplicationContext);
        }
    }
}