// MvxBindingContextOwnerExtensions.Fluent.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static MvxFluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<TTarget, TSource>(
            this TTarget target)
            where TTarget : class, IMvxBindingContextOwner
        {
            return new MvxFluentBindingDescriptionSet<TTarget, TSource>(target);
        }

        /*
         * removed for now - it's too confusing in app code
        public static MvxFluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<TTarget, TSource>(this TTarget target, TSource justUsedForTypeInference)
            where TTarget : class, IMvxBindingContextOwner
        {
            return new MvxFluentBindingDescriptionSet<TTarget, TSource>(target);
        }
         */

        public static MvxFluentBindingDescription<TTarget> CreateBinding<TTarget>(this TTarget target)
            where TTarget : class, IMvxBindingContextOwner
        {
            return new MvxFluentBindingDescription<TTarget>(target, target);
        }

        public static MvxFluentBindingDescription<TTarget> CreateBinding<TTarget>(
            this IMvxBindingContextOwner contextOwner, TTarget target)
            where TTarget : class
        {
            return new MvxFluentBindingDescription<TTarget>(contextOwner, target);
        }
    }
}