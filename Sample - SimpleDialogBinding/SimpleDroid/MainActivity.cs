using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Simple;

namespace SimpleDroid
{
    [Activity(Label = "SimpleDroid", MainLauncher = true, Icon = "@drawable/icon")]
    public sealed class MainActivity : MvxSimpleBindingActivity
    {
        public MainActivity()
        {
            DataContext = new TipViewModel();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

