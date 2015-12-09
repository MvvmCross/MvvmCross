// MvxUIButtonTitleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Target;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
    public abstract class MvxMacTargetBinding : MvxConvertingTargetBinding
    {
        protected MvxMacTargetBinding(object view)
            : base(view)
        {
        }
    }
}