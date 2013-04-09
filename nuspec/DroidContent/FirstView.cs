using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Droid.Views;
using YourNamespace.Core.ViewModels;

namespace YourNamespace.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);
        }
    }
}