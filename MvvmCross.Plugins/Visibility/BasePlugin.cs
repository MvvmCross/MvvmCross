// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using MvvmCross.Binding.Combiners;
using MvvmCross.IoC;

namespace MvvmCross.Plugin.Visibility
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load(IMvxIoCProvider provider)
        {
            if (provider.TryResolve(out IMvxValueCombinerRegistry registry))
                RegisterValueConverters(registry);
        }

        private void RegisterValueConverters(IMvxValueCombinerRegistry registry)
        {
            registry.AddOrOverwriteFrom(GetType().GetTypeInfo().Assembly);
        }
    }
}
