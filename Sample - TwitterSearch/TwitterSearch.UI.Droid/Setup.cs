using System;
using System.Collections.Generic;
using Android.Content;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Binding.Parse.Binding;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using TwitterSearch.Core;
using TwitterSearch.Core.Converters;

namespace TwitterSearch.UI.Droid
{
    public class Setup
        : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override void InitializeFirstChance()
        {
            Mvx.RegisterType<IMvxBindingParser, MvxSwissBindingParser>();
            base.InitializeFirstChance();
        }

        protected override MvxApplication CreateApp()
        {
            return new TwitterSearchApp();
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return new[] { typeof(Converters) }; }
        }

        protected override IMvxNavigationRequestSerializer CreateNavigationRequestSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
            var json = Mvx.Resolve<IMvxJsonConverter>();
            return new MvxNavigationRequestSerializer(json);
        }

        protected override void InitializeLastChance()
        {
            Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            base.InitializeLastChance();
        }
    }
}

