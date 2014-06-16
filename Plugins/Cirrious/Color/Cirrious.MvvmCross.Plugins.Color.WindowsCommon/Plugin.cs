// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.CrossCore.UI;

namespace Cirrious.MvvmCross.Plugins.Color.WindowsCommon
{
    public class Plugin
        : IMvxPlugin
          
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMvxNativeColor>(new MvxWindowsCommonColor());
        }
    }
}