// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.BindingContext
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