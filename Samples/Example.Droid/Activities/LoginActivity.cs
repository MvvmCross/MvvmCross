using Android.App;
using Android.OS;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Support.AppCompat;

namespace Example.Droid.Activities
{
    [Activity(
        Label = "Examples",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
        Name = "example.droid.activities.LoginActivity"
    )]			
    public class LoginActivity : MvxAppCompatActivity
    {
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView ( Resource.Layout.activity_login );
        }
    }
}

