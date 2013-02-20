// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.WinRT
{
    public class Plugin
        : IMvxPlugin
          , IMvxServiceProducer
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceType<IMvxResourceLoader, MvxWinRTResourceLoader>();
        }

        #endregion
    }
}