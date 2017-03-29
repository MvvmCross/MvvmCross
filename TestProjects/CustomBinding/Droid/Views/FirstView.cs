using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace MvvmCross.TestProjects.CustomBinding.Droid.Views
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
