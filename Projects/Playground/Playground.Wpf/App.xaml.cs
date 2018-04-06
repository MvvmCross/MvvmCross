using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Platforms.Wpf.Views;

namespace Playground.Wpf
{
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            base.RegisterSetup();
            MvxSetup.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }
        public override void ApplicationInitialized()
        {
            if (MainWindow == null) return;
            var presenter = new MultiRegionWpfViewPresenter(MainWindow);
            MvxWpfSetupSingleton.EnsureSingletonAvailable(Dispatcher, presenter).EnsureInitialized();

            RunAppStart();
        }
    }
}
