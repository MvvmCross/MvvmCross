using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Touch;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using TwitterSearch.Core;
using TwitterSearch.Core.Converters;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;

namespace TwitterSearch.UI.Touch
{
   public class Setup
        : MvxTouchSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            var app = new TwitterSearchApp();
            return app;
        }

		protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            var filler = new MvxInstanceBasedValueConverterRegistryFiller(registry);
            filler.AddFieldConverters(typeof(Converters));
        }

		protected override void AddPluginsLoaders(MvxLoaderPluginRegistry registry)
		{
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Visibility.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.DownloadCache.Touch.Plugin>();
			base.AddPluginsLoaders(registry);
		}

		protected override void InitializeLastChance ()
		{
			base.InitializeLastChance ();
			Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
			Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
		}
    }
}

