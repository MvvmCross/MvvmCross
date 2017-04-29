using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Views;
using Xamarin.Forms;

namespace MvxBindingsExample.Droid
{
    [Activity(Label = "MvxBindingsExample.Droid",
        MainLauncher = true,
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        private bool isInitializationComplete;

        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }

        public override void InitializationComplete()
        {
            if (!isInitializationComplete)
            {
                isInitializationComplete = true;
                StartActivity(typeof(MvxFormsApplicationActivity));
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            Forms.Init(this, bundle);
            // Leverage controls' StyleId attrib. to Xamarin.UITest
            Forms.ViewInitialized += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                    e.NativeView.ContentDescription = e.View.StyleId;
            };

            base.OnCreate(bundle);
        }
    }
}