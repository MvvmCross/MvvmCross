using System.Windows;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace TwitterSearch.UI.Wpf
{
    public partial class App
        : Application
        , IMvxServiceConsumer
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var start = this.GetService<IMvxStartNavigation>();
            start.Start();
        }
    }
}
