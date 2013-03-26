using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.Visibility;
using Cirrious.MvvmCross.Touch;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers.Touch
{
    public class Setup
        : MvxTouchSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override void AddPluginsLoaders(MvxLoaderPluginRegistry registry)
        {
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.Touch.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.DownloadCache.Touch.Plugin>();
            registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.Touch.Plugin>();
            base.AddPluginsLoaders(registry);
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.Visibility.PluginLoader>();
            pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.Json.PluginLoader>();
            pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader>();
            pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.File.PluginLoader>();
            base.LoadPlugins(pluginManager);
        }

        protected override IMvxApplication CreateApp()
        {
            var app = new App();
            return app;
        }

		protected override void InitializeLastChance()
		{
			// create a new error displayer - it will hook itself into the framework
			var errorDisplayer = new ErrorDisplayer();

			base.InitializeLastChance();
		}
    }
}