#region Copyright
// <copyright file="MvxBaseTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public abstract class MvxBaseTargetBinding  : MvxBaseBinding, IMvxTargetBinding
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