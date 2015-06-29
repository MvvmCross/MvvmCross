using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Support.AppCompat;

namespace Example.Droid.Views
{
    [Activity(Label = "View for ListViewModel")]
    public class ListView : MvxActivityCompat
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ListView);
        }
    }
}