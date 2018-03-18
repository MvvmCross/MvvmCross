using System;
using System.Windows;
using MvvmCross.Core;
using MvvmCross.Platform.Wpf.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platform.Wpf.Views
{
    public abstract class MvxApplication : Application 
    {
        protected IMvxWpfSetup Setup
        {
            get
            {
                return MvxSetup.PlatformInstance<IMvxWpfSetup>();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            Setup.PlatformInitialize(Dispatcher, MainWindow);
            Setup.Initialize();

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
