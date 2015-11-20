// MvxElementValueTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using CrossUI.Droid.Dialog.Elements;
using System;
using System.Reflection;

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
                                      "Error - ValueElement is null in MvxElementValueTargetBinding");
            }
            else
            {
                valueElement.ValueChanged += ElementOnValueChanged;
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

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