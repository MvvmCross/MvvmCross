// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Converters;
using MvvmCross.IoC;

namespace MvvmCross.Plugin.Visibility;

public abstract class BasePlugin : IMvxPlugin
{
    public virtual void Load(IMvxIoCProvider provider)
    {
        if (provider.TryResolve(out IMvxValueConverterRegistry registry))
            RegisterValueConverters(registry);
    }

    private static void RegisterValueConverters(IMvxValueConverterRegistry registry)
    {
        registry.AddOrOverwrite("Visibility", new MvxVisibilityValueConverter());
        registry.AddOrOverwrite("InvertedVisibility", new MvxInvertedVisibilityValueConverter());
    }
}
