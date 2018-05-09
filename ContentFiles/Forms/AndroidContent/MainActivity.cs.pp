using Android.App;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views;

namespace $rootnamespace$
{
    [Activity(Label = "$rootnamespace$", MainLauncher = true, Theme = "@style/MainTheme", NoHistory = true)]
    public class MainActivity : MvxFormsAppCompatActivity<MvxFormsAndroidSetup<Core.App, FormsUI.App>, Core.App, FormsUI.App>
    {
    }
}