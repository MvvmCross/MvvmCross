using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MvvmCross.Droid.Views;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Droid
{
    [Activity(Label = "A", Icon = "@mipmap/icon")]
    public class TestAView : MvxActivity<TestAViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Test);

            var text = FindViewById<TextView>(Resource.Id.text);
            text.Text = "A";

            var layout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

            layout.SetBackgroundColor(Color.MediumPurple);
        }
    }
}
