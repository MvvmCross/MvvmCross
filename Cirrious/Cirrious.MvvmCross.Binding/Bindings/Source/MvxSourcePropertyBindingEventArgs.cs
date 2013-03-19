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
        private readonly bool _isAvailable;

        private readonly object _value;

        public MvxSourcePropertyBindingEventArgs(bool isAvailable, Object value)
        {
            _isAvailable = isAvailable;
            _value = value;
        }

        public MvxSourcePropertyBindingEventArgs(IMvxSourceBinding propertySourceBinding)
        {
            _isAvailable = propertySourceBinding.TryGetValue(out _value);
        }

        public bool IsAvailable
        {
            get { return _isAvailable; }
        }

        public object Value
        {
            get { return _value; }
        }
    }
}