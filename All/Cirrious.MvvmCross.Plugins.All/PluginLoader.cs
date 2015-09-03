// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;

namespace MvvmCross.Plugins.All
{
    public class PluginLoader
        : IMvxPluginLoader
    {
        private bool _loaded;
        public static readonly PluginLoader Instance = new PluginLoader();

        private static readonly Type[] AllPluginTypes = new Type[]
            {
                typeof(Cirrious.MvvmCross.Plugins.Accelerometer.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Bookmarks.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Color.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Email.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.File.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Json.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.JsonLocalisation.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Location.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Messenger.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Network.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.PhoneCall.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.PictureChooser.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.ReflectionEx.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.ResourceLoader.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Share.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.SoundEffects.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Sqlite.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.ThreadUtils.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.Visibility.PluginLoader),
                typeof(Cirrious.MvvmCross.Plugins.WebBrowser.PluginLoader),
            };

        public void EnsureLoaded()
        {
            if (_loaded)
                return;

            _loaded = true;

            MvxTrace.Trace("Loading all plugins");

            var loaded = new List<Type>();
            var failed = new List<Type>();

            var manager = Mvx.Resolve<IMvxPluginManager>();
            foreach (var type in AllPluginTypes)
            {
                if (OptionalLoadPlatformAdaption(manager, type))
                {
                    loaded.Add(type);
                }
                else
                {
                    failed.Add(type);
                }
            }

            MvxTrace.Trace("Plugins loaded: {0}", string.Join("\n", loaded.Select(x => x.FullName)));
            MvxTrace.Trace("Plugins failed: {0}", string.Join("\n", failed.Select(x => x.FullName)));
        }

        private bool OptionalLoadPlatformAdaption(IMvxPluginManager manager, Type type)
        {
            try
            {
                manager.EnsurePluginLoaded(type);
                return true;
            }
            catch (Exception exception /*Pokemon - catch them all*/)
            {
                MvxTrace.Trace("Exception loading {0} was {1}", type.FullName, exception.ToLongString());
                return false;
            }
        }
    }
}