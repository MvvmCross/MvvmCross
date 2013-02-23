using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cirrious.MvvmCross.Droid.Simple;

namespace SimpleDroidSql
{
    [Activity(Label = "SimpleDroidSql", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public sealed class MainActivity 
        : MvxSimpleBindingActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            DataContext = new ListViewModel();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

