using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.Conference.Core;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace Cirrious.Conference.UI.WP7
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
            return new ConferenceApp();
        }

        protected override void InitializeDefaultTextSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.ModuleLoader.Instance.EnsureLoaded(true);
        }

        protected override void AddPluginsLoaders(MvxWindowsPhonePluginLoaderRegistry registry)
        {
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Email.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.PhoneCall.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ResourceLoader.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Share.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.WindowsPhone.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.WebBrowser.WindowsPhone.Plugin>();
            base.AddPluginsLoaders(registry);
        }
    }
}
