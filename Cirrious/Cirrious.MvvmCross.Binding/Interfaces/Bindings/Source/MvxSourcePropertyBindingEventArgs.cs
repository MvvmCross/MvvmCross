#region Copyright
// <copyright file="MvxSourcePropertyBindingEventArgs.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source
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