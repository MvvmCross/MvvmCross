namespace RoutingExample.Droid
{
    [Activity(Label = "A", Icon = "@drawable/icon")]
    public class TestBView : MvxActivity<TestBViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            SetContentView(Resource.Layout.Test);

            var text = FindViewById<TextView>(Resource.Id.text);
            text.Text = "B";

            var layout = FindViewById<RelativeLayout>(Resource.Id.layout);
            layout.SetBackgroundColor(Color.Green);
        }
    }
}