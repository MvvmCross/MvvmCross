using System;
using System.Collections.Generic;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.Plugins.Visibility;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace BestSellers.Droid
{
    public class Setup
        : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override void InitializeLastChance()
        {
            var errorHandler = new ErrorDisplayer(ApplicationContext);
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            base.InitializeLastChance();
        }
    }
}