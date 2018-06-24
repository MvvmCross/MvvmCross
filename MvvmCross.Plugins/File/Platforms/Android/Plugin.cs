// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android;
using MvvmCross.Plugin.File;

namespace MvvmCross.Plugin.File.Platforms.Android
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : IMvxConfigurablePlugin
    {
        private MvxFileConfiguration _configuration;
        private MvxFileConfiguration Configuration => _configuration ?? new MvxFileConfiguration(
            Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>().ApplicationContext.FilesDir.Path
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
            var fileStore = new MvxIoFileStoreBase(Configuration.AppendDefaultPath, Configuration.BasePath);

            Mvx.IoCProvider.RegisterSingleton<IMvxFileStore>(fileStore);
            Mvx.IoCProvider.RegisterSingleton<IMvxFileStoreAsync>(fileStore);
        }
    }
}
