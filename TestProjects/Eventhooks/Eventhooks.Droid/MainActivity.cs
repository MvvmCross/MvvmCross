using Android.App;
using Android.OS;
using Android.Widget;
using Eventhooks.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Binding.BindingContext;

namespace Eventhooks.Droid
{
    [Activity(Label = "Eventhooks", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : MvxActivity<FirstViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            var bindingSet = this.CreateBindingSet<MainActivity, FirstViewModel>();
            bindingSet.Bind(button).To(vm => vm.ShowSecondView);
            bindingSet.Apply();
        }
    }
}
