using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;
using MyApplication.Core.Interfaces.Errors;
using Windows.UI.Popups;

namespace MyApplication.UI.WinRT
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
            var dialog = new MessageDialog(message);
            dialog.ShowAsync();
        }
    }
}