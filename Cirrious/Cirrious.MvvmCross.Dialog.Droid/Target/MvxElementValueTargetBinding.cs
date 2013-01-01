#region Copyright

// <copyright file="MvxElementValueTargetBinding.cs" company="Cirrious">
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
using CrossUI.Droid.Dialog.Elements;

namespace Cirrious.MvvmCross.Dialog.Droid.Target
{
    public class MvxElementValueTargetBinding : MvxPropertyInfoTargetBinding<ValueElement>
    {
        public MvxElementValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var valueElement = View;
            if (valueElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - ValueElement is null in MvxEditTextTextTargetBinding");
            }
            else
            {
                valueElement.ValueChanged += ElementOnValueChanged;
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        private void ElementOnValueChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.Value);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null)
                {
                    editText.ValueChanged -= ElementOnValueChanged;
                }
            }
        }
    }
}