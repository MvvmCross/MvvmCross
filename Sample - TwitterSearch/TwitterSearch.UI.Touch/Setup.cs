using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using TwitterSearch.Core;
using TwitterSearch.Core.Converters;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;

namespace TwitterSearch.UI.Touch
{
   public class Setup
        : MvxTouchBindingSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        #region Overrides of MvxBaseSetup
				
        protected override MvxApplication CreateApp()
        {
            var app = new TwitterSearchApp();
            return app;
        }

		protected override void InitializeFirstChance ()
		{
			Mvx.RegisterType<IMvxBindingParser, MvxSwissBindingParser>();
			base.InitializeFirstChance ();
		}

		protected override void FillValueConverters(Cirrious.MvvmCross.Binding.Interfaces.Binders.IMvxValueConverterRegistry registry)
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
		}

        #endregion
    }

}

