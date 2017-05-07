// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using System;
using System.IO;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Plugins.File.Wpf
{
    public class Plugin
        : IMvxConfigurablePlugin
    {
        private WpfFileStoreConfiguration _configuration;
        private WpfFileStoreConfiguration Configuration => _configuration ?? WpfFileStoreConfiguration.Default;

        public void Load()
        {
            var fileStore = new MvxWpfFileStore(Configuration.AppendDefaultPath, Configuration.RootFolder);
            Mvx.RegisterSingleton<IMvxFileStore>(fileStore);
            Mvx.RegisterSingleton<IMvxFileStoreAsync>(fileStore);
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration == null) return;

            var wpfConfiguration = configuration as WpfFileStoreConfiguration;
            if (wpfConfiguration == null)
            {
                throw new MvxException("You must use a WpfFileStoreConfiguration object for configuring the File Plugin, but you supplied {0}", configuration.GetType().Name);
            }

            if (!Directory.Exists(wpfConfiguration.RootFolder))
            {
                var message = "File plugin configuration error : root folder '" + wpfConfiguration.RootFolder + "' does not exists.";
                MvxTrace.Error(message);
                throw new DirectoryNotFoundException(message);
            }

            _configuration = wpfConfiguration;
        }
    }
}