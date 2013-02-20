using Cirrious.Conference.Core.Interfaces;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;
using MonoTouch.UIKit;

namespace Cirrious.Conference.UI.Touch
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
            var errorView = new UIAlertView("SQLBits X", message, null, "OK", null);
            errorView.Show();
        }
    }
}