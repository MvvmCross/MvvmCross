using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Touch.Platform;
using Tutorial.Core;
using Tutorial.Core.Converters;
using Tutorial.Core.ViewModels;
using Tutorial.Core.ViewModels.Lessons;
using Tutorial.UI.Touch.Views;
using Tutorial.UI.Touch.Views.Lessons;
using Cirrious.MvvmCross.Platform;

namespace Tutorial.UI.Touch
{
    public class Setup
        : MvxTouchDialogBindingSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app = new App();
            return app;
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return new[] { typeof(Converters) }; }
        }

		protected override void AddPluginsLoaders(MvxLoaderPluginRegistry registry)
		{
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Location.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ThreadUtils.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.DownloadCache.Touch.Plugin>();
			base.AddPluginsLoaders(registry);
		}

		protected override void InitializeLastChance ()
		{
			base.InitializeLastChance ();
			Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
		}
    }
}
