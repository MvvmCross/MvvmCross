using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.UI.Droid
{
    public class ErrorDisplayer
    {
        private readonly Context _applicationContext;

        public ErrorDisplayer(Context applicationContext)
        {
            _applicationContext = applicationContext;

            var source = Mvx.Resolve<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity as IMvxBindingContextOwner;
            var alertDialog = new AlertDialog.Builder((Activity)activity).Create();
            alertDialog.SetTitle("Sorry!");
            alertDialog.SetMessage(message);
            alertDialog.SetButton("OK", (sender, args) => {});
            alertDialog.Show();
        }
    }
}