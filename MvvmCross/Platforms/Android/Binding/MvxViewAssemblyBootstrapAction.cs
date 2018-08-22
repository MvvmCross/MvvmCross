// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;

namespace MvvmCross.Platforms.Android.Binding
{
    public class MvxViewAssemblyBootstrapAction<TView>
        : IMvxBootstrapAction
    {
        public virtual void Run()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTypeCache<View>>(RegisterViewTypes);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxNamespaceListViewTypeResolver>(RegisterNamespace);
        }

        protected virtual void RegisterViewTypes()
        {
            var cache = Mvx.IoCProvider.Resolve<IMvxTypeCache<View>>();
            cache.AddAssembly(typeof(TView).Assembly);
        }

        protected virtual void RegisterNamespace()
        {
            var resolver = Mvx.IoCProvider.Resolve<IMvxNamespaceListViewTypeResolver>();
            resolver.Add(typeof(TView).Namespace);
        }
    }
}
