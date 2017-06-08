// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.File.iOS
{
    [Preserve(AllMembers = true)]
    public class Plugin
        : IMvxConfigurablePlugin
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

            Mvx.RegisterSingleton<IMvxFileStore>(fileStore);
            Mvx.RegisterSingleton<IMvxFileStoreAsync>(fileStore);
        }
    }
}