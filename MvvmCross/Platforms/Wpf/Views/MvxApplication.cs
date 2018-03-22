using System;
using System.Windows;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Wpf.Views
{
    public abstract class MvxApplication : Application 
    {
        protected override void OnActivated(EventArgs e)
        {
            MvxWpfSetupSingleton.EnsureSingletonAvailable(Dispatcher, MainWindow).EnsureInitialized();

            RunAppStart(e);
            base.OnActivated(e);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(GetAppStartHint(hint));
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
        }
    }
}
