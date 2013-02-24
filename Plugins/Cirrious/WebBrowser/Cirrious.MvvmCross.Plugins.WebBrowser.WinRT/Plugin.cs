// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Plugins;

namespace Cirrious.MvvmCross.Plugins.WebBrowser.WinRT
{
    public class Plugin
        : IMvxPlugin
          
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            Mvx.RegisterType<IMvxWebBrowserTask, MvxWebBrowserTask>();
        }

        #endregion
    }
}