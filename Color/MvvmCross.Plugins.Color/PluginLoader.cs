// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.Color
{
    public class PluginLoader
        : IMvxPluginLoader
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        public void EnsureLoaded()
        {
            var manager = Mvx.Resolve<IMvxPluginManager>();
            manager.EnsurePlatformAdaptionLoaded<PluginLoader>();

            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(RegisterValueConverters);
        }

        private void RegisterValueConverters()
        {
            var registry = Mvx.Resolve<IMvxValueConverterRegistry>();
            registry.AddOrOverwrite("ARGB", new MvxARGBValueConverter());
            registry.AddOrOverwrite("NativeColor", new MvxNativeColorValueConverter());
            registry.AddOrOverwrite("RGBA", new MvxRGBAValueConverter());
            registry.AddOrOverwrite("RGB", new MvxRGBValueConverter());
            registry.AddOrOverwrite("RGBIntColor", new MvxRGBIntColorValueConverter());
        }
    }
}