// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.All
{
    [Preserve(AllMembers = true)]
    public class PluginLoader
        : IMvxPluginLoader
    {
        private bool _loaded;
        public static readonly PluginLoader Instance = new PluginLoader();

        private static readonly Type[] AllPluginTypes = {
                typeof(MvvmCross.Plugins.Accelerometer.PluginLoader),
                typeof(MvvmCross.Plugins.Bookmarks.PluginLoader),
                typeof(MvvmCross.Plugins.Color.PluginLoader),
                typeof(MvvmCross.Plugins.DownloadCache.PluginLoader),
                typeof(MvvmCross.Plugins.Email.PluginLoader),
                typeof(MvvmCross.Plugins.File.PluginLoader),
                typeof(MvvmCross.Plugins.Json.PluginLoader),
                typeof(MvvmCross.Plugins.JsonLocalization.PluginLoader),
                typeof(MvvmCross.Plugins.Location.PluginLoader),
                typeof(MvvmCross.Plugins.Messenger.PluginLoader),
                typeof(MvvmCross.Plugins.MethodBinding.PluginLoader),
                typeof(MvvmCross.Plugins.Network.PluginLoader),
                typeof(MvvmCross.Plugins.PhoneCall.PluginLoader),
                typeof(MvvmCross.Plugins.PictureChooser.PluginLoader),
                typeof(MvvmCross.Plugins.ResourceLoader.PluginLoader),
                typeof(MvvmCross.Plugins.Share.PluginLoader),
                typeof(MvvmCross.Plugins.SoundEffects.PluginLoader),
                typeof(MvvmCross.Plugins.Visibility.PluginLoader),
                typeof(MvvmCross.Plugins.WebBrowser.PluginLoader)
            };

        public void EnsureLoaded()
        {
            if(_loaded)
                return;

            _loaded = true;

            MvxTrace.Trace("Loading all plugins");

            var loaded = new List<Type>();
            var failed = new List<Type>();

            var manager = Mvx.Resolve<IMvxPluginManager>();
            foreach(var type in AllPluginTypes)
            {
                if(OptionalLoadPlatformAdaption(manager, type))
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
            catch(Exception exception /*Pokemon - catch them all*/)
            {
                MvxTrace.Trace("Exception loading {0} was {1}", type.FullName, exception.ToLongString());
                return false;
            }
        }
    }
}