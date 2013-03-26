using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.AutoView.Touch;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.AutoViews.Core;
using CustomerManagement.AutoViews.Core.Interfaces;
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

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
           
            Mvx.RegisterSingleton<IViewModelCloser>(_presenter);
            SetupAutoViews();
        }

        private void SetupAutoViews()
        {
            var autoView = new MvxAutoViewSetup();
            autoView.Initialize();
        }
    }
}