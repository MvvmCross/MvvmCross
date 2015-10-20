using Android.App;
using Android.Content.PM;
using Android.OS;
using Cirrious.MvvmCross.Droid.Support.AppCompat;
using Example.Core.ViewModels;

namespace Example.Droid.Activities
{
    [Activity(
        Label = "Examples",
        Theme = "@style/AppTheme.Login",
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
        Name = "example.droid.activities.LoginActivity"
    )]			
    public class LoginActivity : MvxAppCompatActivity<LoginViewModel>
    {
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView ( Resource.Layout.activity_login );
        }
    }
}

