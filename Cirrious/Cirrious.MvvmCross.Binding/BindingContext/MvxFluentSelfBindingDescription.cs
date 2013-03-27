// MvxFluentSelfBindingDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxFluentSelfBindingDescription<TTarget>
        : MvxFluentBindingDescription<TTarget>
        where TTarget : class, IMvxBindingContextOwner
    {
        public MvxFluentSelfBindingDescription(TTarget bindingContextOwner)
            : base(bindingContextOwner, bindingContextOwner)
        {
        }
    }
}