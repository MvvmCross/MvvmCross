using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Plugin;
using Serilog;
using Serilog.Extensions.Logging;

namespace Playground.Wpf
{
    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Debug()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Messenger.Plugin>();
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Json.Plugin>();
        }

        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            var list = new List<Assembly>();
            list.AddRange(base.GetViewAssemblies());
            list.Add(typeof(Playground.Wpf.UI.Views.RootView).Assembly);
            return list.ToArray();
        }
    }
}
