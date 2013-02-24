// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.UI;

namespace Cirrious.MvvmCross.Plugins.Visibility.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
          , IMvxProducer
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceInstance<IMvxNativeVisibility>(new MvxWindowsPhoneVisibility());
        }

        #endregion
    }
}