﻿// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.Json
{
    [Preserve(AllMembers = true)]
    public class PluginLoader
        : IMvxConfigurablePluginLoader
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        private bool _loaded;
        private MvxJsonConfiguration _configuration;

        public void EnsureLoaded()
        {
            if (_loaded)
                return;

            _loaded = true;
            Mvx.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
            var configuration = _configuration ?? MvxJsonConfiguration.Default;

            if (configuration.RegisterAsTextSerializer)
            {
                Mvx.RegisterType<IMvxTextSerializer, MvxJsonConverter>();
            }
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (_loaded)
            {
                var log = Mvx.Resolve<IMvxLogProvider>().GetLogFor<PluginLoader>();
                log.Error("Error - Configure called for Json Plugin after the plugin is already loaded");
                return;
            }

            if (configuration != null && !(configuration is MvxJsonConfiguration))
            {
                throw new MvxException("You must configure the Json plugin with MvxJsonConfiguration - but supplied {0}", configuration.GetType().Name);
            }

            _configuration = (MvxJsonConfiguration)configuration;
        }
    }
}