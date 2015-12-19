// MvxElementValueTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Droid.Target
{
    using System;
    using System.Reflection;

    using CrossUI.Droid.Dialog.Elements;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

    public class MvxElementValueTargetBinding : MvxPropertyInfoTargetBinding<ValueElement>
    {
        public MvxElementValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var valueElement = this.View;
            if (valueElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - ValueElement is null in MvxElementValueTargetBinding");
            }
            else
            {
                valueElement.ValueChanged += this.ElementOnValueChanged;
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        private void ElementOnValueChanged(object sender, EventArgs eventArgs)
        {
            this.FireValueChanged(this.View.Value);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = this.View;
                if (editText != null)
                {
                    editText.ValueChanged -= this.ElementOnValueChanged;
                }
            }
        }
    }
}