using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cirrious.MvvmCross.Binding.Droid.Simple;

namespace SimpleDroidSql
{
    [Activity(Label = "SimpleDroidSql", MainLauncher = true, Icon = "@drawable/icon")]
    public sealed class MainActivity : MvxSimpleBindingActivity<ListViewModel>
    {
        public MainActivity()
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (ViewModel == null)
                ViewModel = new ListViewModel();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

