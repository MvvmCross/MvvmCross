using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.AutoView.Touch;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Platform;
using CustomerManagement.AutoViews.Core;
using CustomerManagement.Touch.Views;

namespace CustomerManagement.Touch
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

		protected override void AddPluginsLoaders(MvxLoaderPluginRegistry registry)
		{
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.PhoneCall.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.WebBrowser.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ResourceLoader.Touch.Plugin>();
			registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.DownloadCache.Touch.Plugin>();
			base.AddPluginsLoaders(registry);
		}

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
			Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            SetupAutoViews();
        }

        private void SetupAutoViews()
        {
            var autoView = new MvxAutoViewSetup();
            autoView.Initialize();
        }
    }
}