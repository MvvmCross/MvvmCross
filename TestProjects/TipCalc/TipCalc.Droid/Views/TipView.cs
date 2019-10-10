
using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace TipCalc.Droid.Views
{
    [Activity(Label = "FirstView")]
    public class TipView : MvxActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FirstView);
            // Create your application here
        }
    }
}