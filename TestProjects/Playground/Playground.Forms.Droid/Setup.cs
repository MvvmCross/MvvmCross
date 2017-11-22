using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Droid.Platform;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;
using Serilog;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Plugins.Json;

namespace Playground.Forms.Droid
{
    public class Setup : MvxTypedFormsAndroidSetup<FormsApp, Core.App>
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Serilog;

        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                .CreateLogger();
            return base.CreateLogProvider();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
        protected override void PerformBootstrapActions()
        {
            base.PerformBootstrapActions();

            PluginLoader.Instance.EnsureLoaded();
        }
    }
}
