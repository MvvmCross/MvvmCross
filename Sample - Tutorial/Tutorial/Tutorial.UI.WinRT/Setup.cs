using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WinRT.Platform;
using Tutorial.Core.ViewModels;
using Tutorial.Core.ViewModels.Lessons;
using Windows.UI.Xaml.Controls;

namespace Tutorial.UI.WinRT
{
    public class Setup
        : MvxWinRtSetup
    {
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app = new Tutorial.Core.App();
            return app;
        }

        protected override void AddPluginsLoaders(Cirrious.MvvmCross.Platform.MvxLoaderPluginRegistry loaders)
        {
            loaders.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Location.WinRT.Plugin>();
            loaders.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ThreadUtils.WinRT.Plugin>();
            loaders.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.WinRT.Plugin>();
            base.AddPluginsLoaders(loaders);
        }
    }
}
