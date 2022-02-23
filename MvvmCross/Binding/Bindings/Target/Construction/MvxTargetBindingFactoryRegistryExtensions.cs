// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public static class MvxTargetBindingFactoryRegistryExtensions
    {
        public static void RegisterCustomBindingFactory<TView>(
            this IMvxTargetBindingFactoryRegistry registry,
            string customName,
            Func<TView, IMvxTargetBinding> creator)
            where TView : class
        {
            registry.RegisterFactory(new MvxCustomBindingFactory<TView>(customName, creator));
        }

        public static void RegisterPropertyInfoBindingFactory(this IMvxTargetBindingFactoryRegistry registry,
                                                              Type bindingType, Type targetType, string targetName)
        {
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(bindingType, targetType, targetName));
        }
    }
}
