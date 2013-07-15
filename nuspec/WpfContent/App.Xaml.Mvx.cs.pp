using System;
using System.Windows;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Views;

namespace $rootnamespace$
{
    public partial class App : Application
    {
        private bool _setupComplete;

        private void DoSetup()
        {
			LoadMvxAssemblyResources();
			
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
		
        private void LoadMvxAssemblyResources()
        {
            for (var i = 0;; i++)
            {
                string key = "MvxAssemblyImport" + i;
                var data = TryFindResource(key);
                if (data == null)
                    return;
            }
        }
    }
}