// IMvxSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source
{
    using System;

    public interface IMvxSourceBinding : IMvxBinding
    {
        Type SourceType { get; }

        void SetValue(object value);

        event EventHandler Changed;

        object GetValue();
    }
}