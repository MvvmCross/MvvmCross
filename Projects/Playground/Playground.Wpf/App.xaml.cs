using System;
using System.Windows;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Playground.Wpf
{
    public partial class App : Application
    {
        bool _setupComplete = false;

        void DoSetup()
        {
            // Hint: You can also set a ContentControl of the Window.
            // var setup = new Setup(Dispatcher, MainWindow.FindName("FooContentControl") as ContentControl);
            var setup = new Setup(Dispatcher, MainWindow);
            setup.Initialize();

            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            _setupComplete = true;
        }

        protected override void OnActivated(EventArgs e)
        {
            if (!_setupComplete)
                DoSetup();

            base.OnActivated(e);
        }
    }
}
