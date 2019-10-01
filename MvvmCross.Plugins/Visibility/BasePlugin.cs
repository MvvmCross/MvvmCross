// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using MvvmCross.Converters;

namespace MvvmCross.Plugin.Visibility
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(RegisterValueConverters);
        }

        private void RegisterValueConverters()
        {
            var registry = Mvx.IoCProvider.Resolve<IMvxValueConverterRegistry>();
            registry.AddOrOverwriteFrom(GetType().GetTypeInfo().Assembly);
        }
    }
}
