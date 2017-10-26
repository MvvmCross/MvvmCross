using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Logging;
using Serilog;

namespace Logging.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override void InitializeFirstChance()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .WriteTo.AndroidLog()
                .CreateLogger();
        }

        protected override IMvxApplication CreateApp()
            => new Core.App();

        protected override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Serilog;
    }
}
