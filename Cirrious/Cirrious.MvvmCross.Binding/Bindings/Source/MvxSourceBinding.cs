// MvxSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxSourceBinding
        : MvxBinding
          , IMvxSourceBinding
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
            var handler = Changed;
            handler?.Invoke(this, EventArgs.Empty);
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