// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Base;
using MvvmCross.Base.Exceptions;
using MvvmCross.Base.Logging;
using MvvmCross.Base.Plugins;

namespace MvvmCross.Plugin.All
{
#pragma warning disable CS0436 // Type conflicts with imported type
    [Preserve(AllMembers = true)]
#pragma warning restore CS0436 // Type conflicts with imported type
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

            MvxPluginLog.Instance.Trace("Loading all plugins");

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

            MvxPluginLog.Instance.Trace("Plugins loaded: {0}", string.Join("\n", loaded.Select(x => x.FullName)));
            MvxPluginLog.Instance.Trace("Plugins failed: {0}", string.Join("\n", failed.Select(x => x.FullName)));
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
                MvxPluginLog.Instance.Trace("Exception loading {0} was {1}", type.FullName, exception.ToLongString());
                return false;
            }
        }
    }
}
