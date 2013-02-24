using Cirrious.CrossCore.Interfaces.IoC;
using MyApplication.Core.Interfaces.Errors;
using Windows.UI.Popups;

namespace MyApplication.UI.WinRT
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
            var dialog = new MessageDialog(message);
            dialog.ShowAsync();
        }
    }
}