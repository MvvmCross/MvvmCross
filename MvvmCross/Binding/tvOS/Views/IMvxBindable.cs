// IMvxBindable.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.tvOS.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platform.Core;

    public interface IMvxBindable
        : IMvxBindingContextOwner
          , IMvxDataConsumer
    {
    }
}