using System;
using System.Collections.Generic;
using Android.Content;
using Cirrious.CrossCore.IoC;
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

        public class Converters
        {
            public readonly MvxVisibilityConverter Visibility = new MvxVisibilityConverter();
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return new[] {typeof (Converters)}; }
        }

        protected override IMvxNavigationSerializer CreateNavigationSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
            return new MvxJsonNavigationSerializer();
        }

        protected override void InitializeLastChance()
        {
            var errorHandler = new ErrorDisplayer(ApplicationContext);
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            base.InitializeLastChance();
        }
    }
}