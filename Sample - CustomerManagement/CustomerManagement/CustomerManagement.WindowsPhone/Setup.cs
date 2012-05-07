using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using CustomerManagement.Core.ViewModels;
using Microsoft.Phone.Controls;

namespace CustomerManagement.WindowsPhone
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
            var app = new Core.App();
            return app;
        }

        protected override void AddPluginsLoaders(MvxWindowsPhonePluginLoaderRegistry registry)
        {
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.PhoneCall.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ResourceLoader.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.WebBrowser.WindowsPhone.Plugin>();
            base.AddPluginsLoaders(registry);
        }
    }
}
