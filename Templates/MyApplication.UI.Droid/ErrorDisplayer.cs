using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.UI.Droid
{
    public class ErrorDisplayer
        : IMvxServiceConsumer
    {
        private readonly Context _applicationContext;

        public ErrorDisplayer(Context applicationContext)
        {
            _applicationContext = applicationContext;

            var source = this.GetService<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            var activity = this.GetService<IMvxAndroidCurrentTopActivity>().Activity as IMvxBindingContextOwner;
            var alertDialog = new AlertDialog.Builder((Activity)activity).Create();
            alertDialog.SetTitle("Sorry!");
            alertDialog.SetMessage(message);
            alertDialog.SetButton("OK", (sender, args) => {});
            alertDialog.Show();
        }
    }
}