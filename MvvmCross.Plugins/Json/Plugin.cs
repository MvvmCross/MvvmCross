// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.IoC;

namespace MvvmCross.Plugin.Json
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxConfigurablePlugin
    {
        private MvxJsonConfiguration _configuration;

        public void Load(IMvxIoCProvider provider)
        {
            provider.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
            var configuration = _configuration ?? MvxJsonConfiguration.Default;

            if (configuration.RegisterAsTextSerializer)
            {
                provider.RegisterType<IMvxTextSerializer, MvxJsonConverter>();
            }
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && configuration is not MvxJsonConfiguration)
            {
                throw new MvxException("You must configure the Json plugin with MvxJsonConfiguration - but supplied {0}", configuration.GetType().Name);
            }

            _configuration = (MvxJsonConfiguration)configuration;
        }
    }
}
