// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.Location.WindowsStore
{
    public class Plugin
        : IMvxPlugin

    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMvxLocationWatcher>(() => new MvxStoreLocationWatcher());
            // ReSharper disable CSharpWarnings::CS0612
            Mvx.RegisterSingleton<IMvxGeoLocationWatcher>(() => new MvxStoreGeoLocationWatcher());
            // ReSharper restore CSharpWarnings::CS0612
        }
    }
}