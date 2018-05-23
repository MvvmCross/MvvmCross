using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Label = "View for CustomBindingViewModel")]
    public class CustomBindingView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CustomBindingView);
        }
    }
}
