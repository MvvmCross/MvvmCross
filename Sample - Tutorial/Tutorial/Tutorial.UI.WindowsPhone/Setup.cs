using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.MvvmCross.Plugins.Location.WindowsPhone;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;
using Tutorial.Core.ViewModels;
using Tutorial.Core.ViewModels.Lessons;
using Tutorial.UI.WindowsPhone.Views;
using Tutorial.UI.WindowsPhone.Views.Lessons;

namespace Tutorial.UI.WindowsPhone
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

        protected override void AddPluginsLoaders(Dictionary<string, Func<Cirrious.MvvmCross.Interfaces.Plugins.IMvxPlugin>> loaders)
        {
            loaders.Add(typeof(Cirrious.MvvmCross.Plugins.Location.PluginLoader).Namespace, () => new Cirrious.MvvmCross.Plugins.Location.WindowsPhone.Plugin());
            loaders.Add(typeof(Cirrious.MvvmCross.Plugins.Visibility.PluginLoader).Namespace, () => new Cirrious.MvvmCross.Plugins.Visibility.WindowsPhone.Plugin());
            base.AddPluginsLoaders(loaders);
        }

        protected override void InitializeLastChance()
        {
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();

            base.InitializeLastChance();
        }
    }
}
