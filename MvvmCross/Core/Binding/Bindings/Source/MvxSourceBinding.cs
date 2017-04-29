// MvxSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxSourceBinding
        : MvxBinding
            , IMvxSourceBinding
    {
        protected MvxSourceBinding(object source)
        {
            Source = source;
        }

        protected object Source { get; }

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