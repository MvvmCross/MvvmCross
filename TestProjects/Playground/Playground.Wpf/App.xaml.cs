using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Wpf.Views.Presenters;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Playground.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        bool _setupComplete = false;

        void DoSetup()
        {
            var presenter = new MvxWpfViewPresenter(MainWindow);
            // Hint: You can also set a ContentControl of the Window.
            // var presenter = new MvxWpfViewPresenter(MainWindow.FindName("FooContentControl") as ContentControl);

            var setup = new Setup(Dispatcher, presenter);
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
