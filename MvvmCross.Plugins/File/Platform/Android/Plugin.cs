// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.File.Droid
{
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxConfigurablePlugin
    {
        private MvxFileConfiguration _configuration;
        private MvxFileConfiguration Configuration => _configuration ?? new MvxFileConfiguration(
            Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext.FilesDir.Path
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

            Mvx.RegisterSingleton<IMvxFileStore>(fileStore);
            Mvx.RegisterSingleton<IMvxFileStoreAsync>(fileStore);
        }
    }
}