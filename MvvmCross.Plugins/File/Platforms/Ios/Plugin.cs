// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;

namespace MvvmCross.Plugin.File.Platforms.Ios
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxConfigurablePlugin
    {
        private MvxFileConfiguration _configuration;
        private MvxFileConfiguration Configuration => _configuration ?? new MvxFileConfiguration(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        );

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration == null) return;

            var fileConfiguration = configuration as MvxFileConfiguration;
            if (fileConfiguration == null)
            {
                throw new MvxException("You must use a MvxFileConfiguration object for configuring the File Plugin, but you supplied {0}", configuration.GetType().Name);
            }

            _configuration = fileConfiguration;
        }

        public void Load()
        {
            var fileStore = new MvxIosFileStore(Configuration.AppendDefaultPath, Configuration.BasePath);

            Mvx.IoCProvider.RegisterSingleton<IMvxFileStore>(fileStore);
            Mvx.IoCProvider.RegisterSingleton<IMvxFileStoreAsync>(fileStore);
        }
    }
}
