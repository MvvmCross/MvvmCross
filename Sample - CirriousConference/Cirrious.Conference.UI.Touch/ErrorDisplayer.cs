using Cirrious.Conference.Core.Interfaces;
using Cirrious.CrossCore.Interfaces.IoC;
using MonoTouch.UIKit;

namespace Cirrious.Conference.UI.Touch
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
            var errorView = new UIAlertView("SQLBits X", message, null, "OK", null);
            errorView.Show();
        }
    }
}