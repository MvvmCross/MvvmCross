// MvxBindingContextOwnerExtensions.Clear.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.BindingContext
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