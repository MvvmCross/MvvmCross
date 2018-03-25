﻿using System.Windows;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Wpf.Views
{
    public abstract class MvxApplication : Application
    {
        public MvxApplication() : base()
        {
            RegisterSetup();
        }

        public virtual void ApplicationInitialized()
        {
            if (MainWindow == null) return;

            MvxWpfSetupSingleton.EnsureSingletonAvailable(Dispatcher, MainWindow).EnsureInitialized();

            RunAppStart();
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

        protected abstract void RegisterSetup();
    }
}
