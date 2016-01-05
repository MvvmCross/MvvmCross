// MvxSourcePropertyBindingEventArgs.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source
{
    using System;

    public class MvxSourcePropertyBindingEventArgs : EventArgs
    {
        private readonly object _value;

        public MvxSourcePropertyBindingEventArgs(Object value)
        {
            this._value = value;
        }

        public MvxSourcePropertyBindingEventArgs(IMvxSourceBinding propertySourceBinding)
        {
            this._value = propertySourceBinding.GetValue();
        }

        public object Value => this._value;
    }
}