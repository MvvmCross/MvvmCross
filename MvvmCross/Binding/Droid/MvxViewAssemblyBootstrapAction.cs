// MvxViewAssemblyBootstrapAction.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid
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