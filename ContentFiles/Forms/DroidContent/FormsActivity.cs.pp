using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Droid.Views;
using MvvmCross.Platform;

namespace $rootnamespace$
{
    [Activity(Label = "$rootnamespace$", Icon = "@drawable/icon", Theme = "@style/MyTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class FormsActivity : MvxFormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
        }
    }
}
