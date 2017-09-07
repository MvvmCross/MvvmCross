using System;
using Android.App;
using Eventhooks.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Binding.BindingContext;
using Android.Widget;
using Android.OS;

namespace Eventhooks.Droid
{
    [Activity(Label = "Eventhooks - SecondActivity")]
    public class SecondActivity : MvxActivity<SecondViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Second);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            var bindingSet = this.CreateBindingSet<SecondActivity, SecondViewModel>();
            bindingSet.Bind(button).To(vm => vm.CloseCommand);
            bindingSet.Apply();
        }
    }
}
