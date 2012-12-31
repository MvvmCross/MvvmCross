#region Copyright

// <copyright file="MvxValueElementValueBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using CrossUI.Touch.Dialog.Elements;

namespace Cirrious.MvvmCross.Dialog.Touch.Target
{
    public class MvxValueElementValueBinding : MvxPropertyInfoTargetBinding<ValueElement>
    {
        public MvxValueElementValueBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var valueElement = View;
            if (valueElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - valueElement is null in MvxValueElementValueBinding");
            }
            else
            {
                valueElement.ValueChanged += ValueElementOnValueChanged;
            }
        }

        private void ValueElementOnValueChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.ObjectValue);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var valueElement = View;
                if (valueElement != null)
                {
                    valueElement.ValueChanged -= ValueElementOnValueChanged;
                }
            }
            base.Dispose(isDisposing);
        }
    }

    /*
    public class MvxValueElementValueBinding<TValueType> : MvxPropertyInfoTargetBinding<ValueElement<TValueType>>
    {
        public MvxValueElementValueBinding(object target, PropertyInfo targetPropertyInfo) 
            : base(target, targetPropertyInfo)
        {
            var valueElement = View;
            if (valueElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - valueElement is null in MvxValueElementValueBinding");
            }
            else
            {
                valueElement.ValueChanged += ValueElementOnValueChanged;
            }
        }

        private void ValueElementOnValueChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.Value);
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.TwoWay;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var valueElement = View;
                if (valueElement != null)
                {
                    valueElement.ValueChanged -= ValueElementOnValueChanged;
                }
            }
            base.Dispose(isDisposing);
        }
    }
     */
}