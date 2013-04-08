using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.Core;
using CustomerManagement.Core.Interfaces;
using CustomerManagement.Core.ViewModels;
using CustomerManagement.Touch.Views;

namespace CustomerManagement.Touch
{
    public class Setup
        : MvxTouchDialogSetup
    {
        private CustomerManagementPresenter _presenter;

        public Setup(MvxApplicationDelegate applicationDelegate, CustomerManagementPresenter presenter)
            : base(applicationDelegate, presenter)
        {
            _presenter = presenter;
        }

        protected override IMvxApplication CreateApp()
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

		protected override void InitializeLastChance ()
		{
			base.InitializeLastChance ();

            Mvx.RegisterSingleton<IViewModelCloser>(_presenter);
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
		}
    }
}