using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MvvmCross.Core;
using MvvmCross.Platform.Wpf.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platform.Wpf.Views
{
    public class MvxApplication : Application
    {
        private MvxWpfSetup _setup;
        protected MvxWpfSetup Setup
        {
            get
            {
                if (_setup == null)
                    _setup = CreateSetup(Dispatcher, MainWindow);
                return _setup;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            Setup.Initialize();

            RunAppStart(e);
            base.OnActivated(e);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(hint);
        }

        protected virtual MvxWpfSetup CreateSetup(Dispatcher uiThreadDispatcher, ContentControl root)
        {
            return MvxSetupExtensions.CreateSetup<MvxWpfSetup>(this.GetType().Assembly, uiThreadDispatcher, root);
        }
    }
}
