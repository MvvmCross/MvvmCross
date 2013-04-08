using Cirrious.Conference.Core.Interfaces;
using Cirrious.CrossCore;
using MonoTouch.UIKit;

namespace Cirrious.Conference.UI.Touch
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
            var errorView = new UIAlertView("SQLBits X", message, null, "OK", null);
            errorView.Show();
        }
    }
}