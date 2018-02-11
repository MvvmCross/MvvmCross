// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Plugin.Json
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxConfigurablePlugin
    {
        private bool _loaded;
        private MvxJsonConfiguration _configuration;

        public void Load()
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
                MvxPluginLog.Instance.Error("Error - Configure called for Json Plugin after the plugin is already loaded");
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
