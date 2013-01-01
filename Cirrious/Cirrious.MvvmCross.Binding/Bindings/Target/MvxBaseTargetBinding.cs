// MvxBaseTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public abstract class MvxBaseTargetBinding : MvxBaseBinding, IMvxTargetBinding
    {
        #region IMvxTargetBinding Members

        public abstract Type TargetType { get; }
        public abstract void SetValue(object value);

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;
        public abstract MvxBindingMode DefaultMode { get; }

        #endregion

        protected virtual void FireValueChanged(object newValue)
        {
            var handler = ValueChanged;

            if (handler != null)
                handler(this, new MvxTargetChangedEventArgs(newValue));
        }
    }
}