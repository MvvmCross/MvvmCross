// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.Location.WindowsPhone
{
    public class Plugin
        : IMvxPlugin          
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMvxLocationWatcher>(() => new MvxWindowsPhoneLocationWatcher());
// ReSharper disable CSharpWarnings::CS0612
            Mvx.RegisterSingleton<IMvxGeoLocationWatcher>(() => new MvxWindowsPhoneGeoLocationWatcher());
// ReSharper restore CSharpWarnings::CS0612
        }
    }
}