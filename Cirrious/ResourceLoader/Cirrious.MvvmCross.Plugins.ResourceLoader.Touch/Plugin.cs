// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.Touch
{
    public class Plugin
        : IMvxPlugin        
    {
        public void Load()
        {
#warning One day I would like to decouple this implementation from the FileStore plugin
            Plugins.File.PluginLoader.Instance.EnsureLoaded();

            Mvx.RegisterType<IMvxResourceLoader, MvxTouchResourceLoader>();
        }
    }
}