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
                typeof(Accelerometer.PluginLoader),
                typeof(Color.PluginLoader),
                typeof(DownloadCache.PluginLoader),
                typeof(Email.PluginLoader),
                typeof(File.PluginLoader),
                typeof(Json.PluginLoader),
                typeof(JsonLocalization.PluginLoader),
                typeof(Location.PluginLoader),
                typeof(Messenger.PluginLoader),
                typeof(MethodBinding.PluginLoader),
                typeof(Network.PluginLoader),
                typeof(PhoneCall.PluginLoader),
                typeof(PictureChooser.PluginLoader),
                typeof(ResourceLoader.PluginLoader),
                typeof(Share.PluginLoader),
                typeof(Visibility.PluginLoader),
                typeof(WebBrowser.PluginLoader)
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