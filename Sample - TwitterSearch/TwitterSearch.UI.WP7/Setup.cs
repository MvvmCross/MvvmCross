using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;
using TwitterSearch.Core;

namespace TwitterSearch.UI.WP7
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
            return new TwitterSearchApp();
        }

        protected override void InitializeDefaultTextSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.ModuleLoader.Instance.EnsureLoaded(true);
        }

        protected override void AddPluginsLoaders(MvxWindowsPhonePluginLoaderRegistry registry)
        {
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.WindowsPhone.Plugin>();
            base.AddPluginsLoaders(registry);
        }
    }
}
