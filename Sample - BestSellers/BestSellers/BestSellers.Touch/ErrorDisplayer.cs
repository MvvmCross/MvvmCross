using Cirrious.CrossCore.Interfaces.IoC;
using MonoTouch.UIKit;

namespace BestSellers.Touch
{
    public class ErrorDisplayer
        : IMvxConsumer
    {
        public ErrorDisplayer()
        {
            var source = this.Resolve<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            var errorView = new UIAlertView("Best Sellers", message, null, "OK", null);
            errorView.Show();
        }
    }
}