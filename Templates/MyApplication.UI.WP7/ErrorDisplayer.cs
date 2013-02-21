using System.Windows;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.UI.WP7
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
            MessageBox.Show(message);
        }
    }
}