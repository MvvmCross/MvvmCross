using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using Serilog;
using UIKit;

namespace Logging.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }
        
        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NSLog()
                .CreateLogger();
        }

        protected override IMvxApplication CreateApp()
            => new Core.App();

        protected override MvvmCross.Platform.Logging.MvxLogProviderType GetDefaultLogProviderType()
            => MvvmCross.Platform.Logging.MvxLogProviderType.Serilog;
    }
}
