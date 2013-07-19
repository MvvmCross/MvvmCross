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

        protected object Source
        {
            get { return _source; }
        }

        public event EventHandler<MvxSourcePropertyBindingEventArgs> Changed;

        public abstract void SetValue(object value);
        
        public abstract Type SourceType { get; }

        public abstract bool TryGetValue(out object value);

        protected void FireChanged(MvxSourcePropertyBindingEventArgs args)
        {
            var handler = Changed;
            if (handler != null)
                handler(this, args);
        }

        protected bool EqualsCurrentValue(object testValue)
        {
            object existing;
            if (!TryGetValue(out existing))
                return false; // if get fails then assume values are different

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