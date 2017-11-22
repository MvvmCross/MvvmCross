// IMvxBindable.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Core;

namespace MvvmCross.Binding.tvOS.Views
{
    public interface IMvxBindable
        : IMvxBindingContextOwner, IMvxDataConsumer
    {
    }
}