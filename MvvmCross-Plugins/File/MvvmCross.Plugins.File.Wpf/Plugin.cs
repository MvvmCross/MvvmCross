// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.File.Wpf
{
    public class Plugin
        : IMvxConfigurablePlugin
    {
        private MvxFileConfiguration _configuration;
        private MvxFileConfiguration Configuration => _configuration ?? new MvxFileConfiguration(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        );

        public void Load()
        {
            var fileStore = new MvxIoFileStoreBase(Configuration.AppendDefaultPath, Configuration.BasePath);
            Mvx.RegisterSingleton<IMvxFileStore>(fileStore);
            Mvx.RegisterSingleton<IMvxFileStoreAsync>(fileStore);
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration == null) return;

            var fileConfiguration = configuration as MvxFileConfiguration;
            if (fileConfiguration == null)
            {
                throw new MvxException("You must use a MvxFileConfiguration object for configuring the File Plugin, but you supplied {0}", configuration.GetType().Name);
            }

            if (!Directory.Exists(fileConfiguration.BasePath))
            {
                var message = "File plugin configuration error : root folder '" + fileConfiguration.BasePath + "' does not exists.";
                MvxTrace.Error(message);
                throw new DirectoryNotFoundException(message);
            }

            _configuration = fileConfiguration;
        }
    }
}