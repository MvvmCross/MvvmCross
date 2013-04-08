using System;
using System.Collections.Generic;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Binding.Parse.Binding;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using TwitterSearch.Core;

namespace TwitterSearch.UI.Droid
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
            return new TwitterSearchApp();
        }

        protected override void InitializeLastChance()
        {
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            base.InitializeLastChance();
        }
    }
}

