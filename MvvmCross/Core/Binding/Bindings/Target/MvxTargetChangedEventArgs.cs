// MvxTargetChangedEventArgs.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using System;

    public class MvxTargetChangedEventArgs
        : EventArgs
    {
        public MvxTargetChangedEventArgs(object value)
        {
            Value = value;
        }

        public object Value { get; private set; }
    }
}