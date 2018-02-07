using System;
using System.Threading.Tasks;
using System.Windows;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Playground.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        bool _setupComplete = false;

        private async Task DoSetup()
        {
            // Hint: You can also set a ContentControl of the Window.
            // var setup = new Setup(Dispatcher, MainWindow.FindName("FooContentControl") as ContentControl);
            var setup = new Setup(Dispatcher, MainWindow);
            setup.Initialize();

            var start = Mvx.Resolve<IMvxAppStart>();
            await start.StartAsync();

            _setupComplete = true;
        }

        protected async override void OnActivated(EventArgs e)
        {
            if (!_setupComplete)
                await DoSetup();

            base.OnActivated(e);
        }
    }
}
