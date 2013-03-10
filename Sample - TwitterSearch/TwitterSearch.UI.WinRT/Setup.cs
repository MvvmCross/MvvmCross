﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WinRT.Platform;
using TwitterSearch.Core;
using Windows.UI.Xaml.Controls;

namespace TwitterSearch.UI.WinRT
{
    public class Setup
        : MvxWinRTSetup
    {
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app = new TwitterSearchApp();
            return app;
        }

        protected override void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            loaders.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.WinRT.Plugin>();
            base.AddPluginsLoaders(loaders);
        }
    }
}
