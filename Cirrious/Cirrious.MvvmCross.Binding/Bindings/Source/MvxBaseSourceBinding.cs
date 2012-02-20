using System;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxBaseSourceBinding 
        : MvxBaseBinding
        , IMvxSourceBinding
    {
        private readonly object _source;

        protected object Source { get { return _source; } }

        protected MvxBaseSourceBinding(object source)
        {
            _source = source;
        }

        public event EventHandler<MvxSourcePropertyBindingEventArgs> Changed;

        protected void FireChanged(MvxSourcePropertyBindingEventArgs args)
        {
            var handler = Changed;
            if (handler != null)
                handler(this, args);
        }

        public abstract void SetValue(object value);
        public abstract Type SourceType { get; }
        public abstract bool TryGetValue(out object value);
    }
}