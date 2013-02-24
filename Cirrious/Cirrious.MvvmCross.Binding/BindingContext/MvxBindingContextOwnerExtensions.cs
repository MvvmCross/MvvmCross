// MvxBindingContextOwnerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Droid.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.BindingContext
{
    public static class MvxBindingContextOwnerExtensions
    {
        public static void RegisterBinding<T>(this IMvxBaseBindingContextOwner<T> owner, IMvxUpdateableBinding binding)
        {
            owner.BindingContext.RegisterBinding(binding);
        }

        public static void ClearBindings<T>(this IMvxBaseBindingContextOwner<T> owner, T view)
        {
            owner.BindingContext.ClearBindings(view);
        }

        public static void ClearAllBindings<T>(this IMvxBaseBindingContextOwner<T> owner)
        {
            owner.BindingContext.ClearAllBindings();
        }
    }
}