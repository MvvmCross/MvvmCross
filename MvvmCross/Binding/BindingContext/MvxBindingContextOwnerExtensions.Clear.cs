// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.BindingContext
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static void ClearBindings(this IMvxBindingContextOwner owner, object target)
        {
            owner.BindingContext.ClearBindings(target);
        }

        public static void ClearAllBindings(this IMvxBindingContextOwner owner)
        {
            owner.BindingContext.ClearAllBindings();
        }
    }
}
