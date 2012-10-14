using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace MyApplication.UI.WP7
{
    public class Setup
        : MvxBaseWindowsPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) 
            : base(rootFrame)
        {
        }

        protected override MvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override void InitializeDefaultTextSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded(true);
        }

        protected override void AddPluginsLoaders(MvxLoaderPluginRegistry registry)
        {
            // TODO - Initialise any required plugins here:
            // e.g. registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.WindowsPhone.Plugin>();
            base.AddPluginsLoaders(registry);
        }

        protected override void InitializeLastChance()
        {
            var errorDisplayer = new ErrorDisplayer();

            base.InitializeLastChance();
        } 
    }
}
