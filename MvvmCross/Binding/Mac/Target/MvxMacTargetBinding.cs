// MvxUIButtonTitleTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Mac.Target
{
    using MvvmCross.Binding.Bindings.Target;

    public abstract class MvxMacTargetBinding : MvxConvertingTargetBinding
    {
        protected MvxMacTargetBinding(object view)
            : base(view)
        {
        }
    }
}