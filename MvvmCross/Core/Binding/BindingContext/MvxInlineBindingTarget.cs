// MvxInlineBindingTarget.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    public class MvxInlineBindingTarget<TViewModel>
    {
        public MvxInlineBindingTarget(IMvxBindingContextOwner bindingContextOwner)
        {
            this.BindingContextOwner = bindingContextOwner;
        }

        public IMvxBindingContextOwner BindingContextOwner { get; private set; }
    }
}