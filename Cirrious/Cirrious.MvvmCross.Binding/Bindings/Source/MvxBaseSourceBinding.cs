// MvxBaseSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxBaseSourceBinding
        : MvxBaseBinding
          , IMvxSourceBinding
    {
        private readonly object _source;

        protected MvxBaseSourceBinding(object source)
        {
            _source = source;
        }

        protected object Source
        {
            get { return _source; }
        }

        #region IMvxSourceBinding Members

        public event EventHandler<MvxSourcePropertyBindingEventArgs> Changed;

        public abstract void SetValue(object value);
        public abstract Type SourceType { get; }
        public abstract bool TryGetValue(out object value);

        #endregion

        protected void FireChanged(MvxSourcePropertyBindingEventArgs args)
        {
            var handler = Changed;
            if (handler != null)
                handler(this, args);
        }
    }
}