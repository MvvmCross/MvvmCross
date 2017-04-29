// MvxSourcePropertyBindingEventArgs.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Binding.Bindings.Source
{
    public class MvxSourcePropertyBindingEventArgs : EventArgs
    {
        public MvxSourcePropertyBindingEventArgs(object value)
        {
            Value = value;
        }

        public MvxSourcePropertyBindingEventArgs(IMvxSourceBinding propertySourceBinding)
        {
            Value = propertySourceBinding.GetValue();
        }

        public object Value { get; }
    }
}