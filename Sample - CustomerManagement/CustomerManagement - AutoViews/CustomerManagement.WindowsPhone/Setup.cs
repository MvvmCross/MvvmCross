using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using CustomerManagement.AutoViews.Core.Interfaces;
using CustomerManagement.WindowsPhone;
using Microsoft.Phone.Controls;

namespace CustomerManagement.AutoViews.WindowsPhone
{
    public class Setup 
        : MvxWindowsPhoneSetup
    {
        private readonly IViewModelCloser _closer;

        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
            _closer = new ViewModelCloser(rootFrame);
        }

        protected override MvxApplication CreateApp()
        {
            var app = new CustomerManagement.AutoViews.Core.App();
            return app;
        }

        protected override IMvxNavigationRequestSerializer CreateNavigationRequestSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
            var json = Mvx.Resolve<IMvxJsonConverter>();
            return new MvxNavigationRequestSerializer(json);
        }

        protected override void AddPluginsLoaders(MvxLoaderPluginRegistry registry)
        {
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.PhoneCall.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ResourceLoader.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.WebBrowser.WindowsPhone.Plugin>();
            base.AddPluginsLoaders(registry);
        }

        protected override void InitializeLastChance()
        {
            Mvx.RegisterSingleton<IViewModelCloser>(_closer);
            base.InitializeLastChance();
        }
    }
}
