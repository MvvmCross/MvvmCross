using System.Windows;
using Cirrious.CrossCore.Interfaces.IoC;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.UI.WP7
{
    public class ErrorDisplayer
        : IMvxConsumer
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