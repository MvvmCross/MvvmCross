using Android.App;
using Android.Content.PM;
using Android.OS;

using Xamarin.Forms;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Droid;
using MvvmCross.Forms.Droid.Views;

namespace PageRendererExample.UI.Droid
{
    [Activity(Label = "PageRendererApplicationActivity", ScreenOrientation=ScreenOrientation.Portrait)]
    public class PageRendererApplicationActivity : MvxFormsApplicationActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
        }
    }
}