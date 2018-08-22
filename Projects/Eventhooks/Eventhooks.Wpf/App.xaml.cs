using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Wpf.Views;
using System.Windows;
using System;
using MvvmCross.Wpf.Views.Presenters;

namespace Eventhooks.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        bool _setupComplete = false;

        void DoSetup()
        {
            var presenter = new MvxSimpleWpfViewPresenter(MainWindow);

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
