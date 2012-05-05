using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace BestSellers.WindowsPhone
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
            var app = new BestSellers.App();
            return app;
        }

        protected override void AddPluginsLoaders(System.Collections.Generic.Dictionary<string, System.Func<Cirrious.MvvmCross.Interfaces.Plugins.IMvxPlugin>> loaders)
        {
            AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.WindowsPhone.Plugin>(loaders);
            base.AddPluginsLoaders(loaders);
        }

        protected override void InitializeLastChance()
        {
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
            base.InitializeLastChance();
        }

    }
}
