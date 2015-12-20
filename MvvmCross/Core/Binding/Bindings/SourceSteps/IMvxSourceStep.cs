// IMvxSourceStep.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.SourceSteps
{
    using System;

    public interface IMvxSourceStep : IMvxBinding
    {
        Type TargetType { get; set; }
        Type SourceType { get; }

        void SetValue(object value);

        event EventHandler Changed;

        object GetValue();

        object DataContext { get; set; }
    }
}