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
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void TriggerFirstNavigate()
        {
            StartActivity(typeof(BindingsApplicationActivity));
            base.TriggerFirstNavigate();
        }

        protected override void OnCreate(Bundle bundle)
        {
            // Leverage controls' StyleId attrib. to Xamarin.UITest
            Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) => {
                                         if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                                         {
                                             e.NativeView.ContentDescription = e.View.StyleId;
                                         }
                                     };

            base.OnCreate(bundle);
        }
    }
}