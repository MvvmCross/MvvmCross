// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.Location.Touch
{
    public class Plugin
        : IMvxPlugin
          
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMvxLocationWatcher>(() => new MvxTouchLocationWatcher());
#pragma warning disable 618 // 618 is that this interface is obsolete
// ReSharper disable CSharpWarnings::CS0612
            Mvx.RegisterSingleton<IMvxGeoLocationWatcher>(() => new MvxTouchGeoLocationWatcher());
// ReSharper restore CSharpWarnings::CS0612
#pragma warning restore 618 // 618 is that this interface is obsolete
        }
    }
}