using System.Windows;
using Cirrious.CrossCore.IoC;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.UI.WP7
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
            MessageBox.Show(message);
        }
    }
}