﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cirrious.Conference.Core;
using Cirrious.Conference.Core.Converters;
using Cirrious.Conference.UI.Droid.Bindings;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Localization;

namespace Cirrious.Conference.UI.Droid
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
            return new NoSplashScreenConferenceApp();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxCustomBindingFactory<Button>("IsFavorite", (button) => new FavoritesButtonBinding(button)));
        }

		protected override System.Collections.Generic.List<System.Reflection.Assembly> ValueConverterAssemblies 
		{
			get 
			{
				var toReturn = base.ValueConverterAssemblies;
				toReturn.Add(typeof(MvxLanguageConverter).Assembly);
				return toReturn;
			}
		}

        public override void LoadPlugins(CrossCore.Plugins.IMvxPluginManager pluginManager)
        {
            pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.Json.PluginLoader>();
            pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.File.PluginLoader>();
            pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader>();
            base.LoadPlugins(pluginManager);
        }
    }
}

