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

        protected override void AddPluginsLoaders(System.Collections.Generic.Dictionary<string, System.Func<Cirrious.MvvmCross.Interfaces.Plugins.IMvxPlugin>> loaders)
        {
            loaders.Add(
                typeof(Cirrious.MvvmCross.Plugins.Visibility.PluginLoader).Namespace,
                () => new Cirrious.MvvmCross.Plugins.Visibility.WindowsPhone.Plugin());
            base.AddPluginsLoaders(loaders);
        }
    }
}
