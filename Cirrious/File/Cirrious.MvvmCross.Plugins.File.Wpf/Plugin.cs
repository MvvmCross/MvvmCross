// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.File.Wpf
{
    public class Plugin
        : IMvxConfigurablePlugin
    {
        private string _rootFolder;

        public void Load()
        {
            var rootFolder = _rootFolder ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fileStore = new MvxWpfFileStore(rootFolder);
            Mvx.RegisterSingleton<IMvxFileStore>(fileStore);
            Mvx.RegisterSingleton<IMvxFileStoreAsync>(fileStore);
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration == null)
                return;

            var wpfConfiguration = (WpfFileStoreConfiguration)configuration;
            if (!Directory.Exists(wpfConfiguration.RootFolder))
            {
                var message = "File plugin configuration error : root folder '" + wpfConfiguration.RootFolder + "' does not exists.";
                MvxTrace.Error(message);
                throw new DirectoryNotFoundException(message);
            }
            _rootFolder = wpfConfiguration.RootFolder;
        }
    }
}