using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace Example.Droid.Views
{
	[Activity(Label = "View for StartViewModel")]
	public class StartView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.StartView);
        }
    }
}