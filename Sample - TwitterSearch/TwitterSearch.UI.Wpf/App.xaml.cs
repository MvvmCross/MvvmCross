using System.Windows;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace TwitterSearch.UI.Wpf
{
    public partial class App
        : Application
        
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var start = Mvx.Resolve<IMvxStartNavigation>();
            start.Start();
        }
    }
}
