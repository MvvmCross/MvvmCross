// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Converters;
using MvvmCross.IoC;

namespace MvvmCross.Plugin.Color
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load(IMvxIoCProvider provider)
        {
            if (provider.TryResolve<IMvxValueConverterRegistry>(out var registry) && registry != null)
            {
                registry.AddOrOverwrite("ARGB", new MvxARGBValueConverter());
                registry.AddOrOverwrite("NativeColor", new MvxNativeColorValueConverter());
                registry.AddOrOverwrite("RGBA", new MvxRGBAValueConverter());
                registry.AddOrOverwrite("RGB", new MvxRGBValueConverter());
                registry.AddOrOverwrite("RGBIntColor", new MvxRGBIntColorValueConverter());
            }
        }
    }
}
