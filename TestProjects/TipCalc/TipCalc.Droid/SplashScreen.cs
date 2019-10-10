
using Android.App;
using MvvmCross.Droid.Views;

namespace TipCalc.Droid
{
    [Activity(Label = "TipCalc", MainLauncher = true)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {

        }
    }
}