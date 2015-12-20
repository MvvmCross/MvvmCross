// IMvxTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using System;

    public interface IMvxTargetBinding : IMvxBinding
    {
        Type TargetType { get; }
        MvxBindingMode DefaultMode { get; }

        void SetValue(object value);

        event EventHandler<MvxTargetChangedEventArgs> ValueChanged;

        void SubscribeToEvents();
    }
}