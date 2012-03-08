using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cirrious.MvvmCross.Binding.Android.Simple;

namespace SimpleDroid
{
    [Activity(Label = "SimpleDroid", MainLauncher = true, Icon = "@drawable/icon")]
    public sealed class MainActivity : MvxSimpleBindingActivity
    {
        public MainActivity()
        {
            ViewModel = new TipViewModel();
        }

        protected override void OnCreate(Bundle bundle)
        {
            Setup.EnsureInitialised(ApplicationContext);

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

