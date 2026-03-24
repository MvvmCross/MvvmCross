using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Hosting;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Wpf.Views
{
    /// <summary>
    /// Base WPF Application class.
    /// Override <see cref="OnStartup"/> and configure the <see cref="MvxWpfHostBuilder"/> there.
    /// </summary>
    public abstract class MvxApplication : Application
    {
        protected virtual void RunAppStart(object hint = null)
        {
            var startup = MvxHost.Current?.Services.GetService<IMvxAppStart>();
            if (startup != null && !startup.IsStarted)
                startup.Start(hint);
        }

        protected virtual object GetAppStartHint(object hint = null) => hint;
    }
}
