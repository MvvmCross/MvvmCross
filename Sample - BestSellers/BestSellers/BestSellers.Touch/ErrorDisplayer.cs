using Cirrious.CrossCore;
using MonoTouch.UIKit;

namespace BestSellers.Touch
{
    public class ErrorDisplayer
    {
        public ErrorDisplayer()
        {
            var source = Mvx.Resolve<IErrorSource>();
            source.ErrorReported += (sender, args) => ShowError(args.Message);
        }

        private void ShowError(string message)
        {
            var errorView = new UIAlertView("Best Sellers", message, null, "OK", null);
            errorView.Show();
        }
    }
}