// MvxSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source
{
    using System;

    public abstract class MvxSourceBinding
        : MvxBinding
          , IMvxSourceBinding
    {
        private readonly object _source;

        protected MvxSourceBinding(object source)
        {
            this._source = source;
        }

        protected object Source => this._source;

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
            var existing = this.GetValue();

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