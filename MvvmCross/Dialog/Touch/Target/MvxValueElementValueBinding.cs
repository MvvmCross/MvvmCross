// MvxValueElementValueBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Touch.Target
{
    using System;
    using System.Reflection;

    using CrossUI.Touch.Dialog.Elements;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

    public class MvxValueElementValueBinding : MvxPropertyInfoTargetBinding<ValueElement>
    {
        public MvxValueElementValueBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var valueElement = this.View;
            if (valueElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - valueElement is null in MvxValueElementValueBinding");
            }
            else
            {
                valueElement.ValueChanged += this.ValueElementOnValueChanged;
            }
        }

        private void ValueElementOnValueChanged(object sender, EventArgs eventArgs)
        {
            this.FireValueChanged(this.View.ObjectValue);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var valueElement = this.View;
                if (valueElement != null)
                {
                    valueElement.ValueChanged -= this.ValueElementOnValueChanged;
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