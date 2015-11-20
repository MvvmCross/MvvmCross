// MvxTargetBindingFactoryRegistryExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Binding.Bindings.Target.Construction
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