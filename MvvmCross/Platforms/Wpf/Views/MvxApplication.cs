using System.Threading.Tasks;
using System.Windows;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Wpf.Views
{
    public abstract class MvxApplication : Application
    {
        protected MvxApplication() : base()
        {
            RegisterSetup();
        }

        public virtual async Task ApplicationInitialized()
        {
            if (MainWindow == null) return;

            await MvxWpfSetupSingleton.EnsureSingletonAvailable(Dispatcher, MainWindow).EnsureInitialized().ConfigureAwait(false);

            await RunAppStart().ConfigureAwait(false);
        }

        protected virtual async Task RunAppStart(object hint = null)
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                await startup.Start(GetAppStartHint(hint)).ConfigureAwait(false);
            }
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
        }

        protected virtual void RegisterSetup()
        {
        }
    }

    public class MvxApplication<TMvxWpfSetup, TApplication> : MvxApplication
       where TMvxWpfSetup : MvxWpfSetup<TApplication>, new()
       where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxWpfSetup>();
        }
    }
}
