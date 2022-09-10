// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxSourceBinding
        : MvxBinding, IMvxSourceBinding
    {
        private readonly object _source;

        protected MvxSourceBinding(object source)
        {
            _source = source;
        }

        protected object Source => _source;

        public event EventHandler Changed;

        public abstract void SetValue(object value);

        public abstract Type SourceType { get; }

        public abstract object GetValue();

        protected void FireChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        protected bool EqualsCurrentValue(object testValue)
        {
            var existing = GetValue();

            if (testValue == null)
            {
                if (existing == null)
                    return true;

                return false;
            }

            return testValue.Equals(existing);
        }
    }
}
