using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using MonoTouch.UIKit;

namespace BestSellers.Touch
{
    public class ErrorDisplayer
        : IMvxServiceConsumer
    {
        public ErrorDisplayer()
        {
            var source = this.GetService<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            var errorView = new UIAlertView("Best Sellers", message, null, "OK", null);
            errorView.Show();
        }
    }
}