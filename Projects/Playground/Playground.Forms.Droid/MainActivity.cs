using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Droid.Views;
using MvvmCross.Platform;
using Playground.Core.ViewModels;

namespace Playground.Forms.Droid
{
    [Activity(
        Label = "Playground.Forms", 
        Icon = "@mipmap/icon",
        Theme = "@style/AppTheme",
        // MainLauncher = true, // No Splash Screen: Uncomment this lines if removing splash screen
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, 
        LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : MvxFormsAppCompatActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            // No Splash Screen: Uncomment these lines if removing splash screen
            // var startup = Mvx.Resolve<IMvxAppStart>();
            // startup.Start();
            // InitializeForms(bundle);
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(false);
        }
    }
}
