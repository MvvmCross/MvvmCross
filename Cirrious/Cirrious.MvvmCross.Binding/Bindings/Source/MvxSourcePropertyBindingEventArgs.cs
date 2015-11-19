// MvxSourcePropertyBindingEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public class MvxSourcePropertyBindingEventArgs : EventArgs
    {
        private readonly object _value;

        public MvxSourcePropertyBindingEventArgs(Object value)
        {
            _value = value;
        }

        public MvxSourcePropertyBindingEventArgs(IMvxSourceBinding propertySourceBinding)
        {
            _value = propertySourceBinding.GetValue();
        }

        public object Value => _value;
    }
}