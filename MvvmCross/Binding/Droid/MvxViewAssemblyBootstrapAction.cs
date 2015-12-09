// MvxViewAssemblyBootstrapAction.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;

namespace Cirrious.MvvmCross.Binding.Droid
{
    public class MvxViewAssemblyBootstrapAction<TView>
        : IMvxBootstrapAction
    {
        public virtual void Run()
        {
            Mvx.CallbackWhenRegistered<IMvxTypeCache<View>>(RegisterViewTypes);
            Mvx.CallbackWhenRegistered<IMvxNamespaceListViewTypeResolver>(RegisterNamespace);
        }

        protected virtual void RegisterViewTypes()
        {
            var cache = Mvx.Resolve<IMvxTypeCache<View>>();
            cache.AddAssembly(typeof(TView).Assembly);
        }

        protected virtual void RegisterNamespace()
        {
            var resolver = Mvx.Resolve<IMvxNamespaceListViewTypeResolver>();
            resolver.Add(typeof(TView).Namespace);
        }
    }
}