// MvxEntryElementValueBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Touch.Target
{
    using System;
    using System.Reflection;

    using CrossUI.iOS.Dialog.Elements;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

    public class MvxEntryElementValueBinding : MvxPropertyInfoTargetBinding<EntryElement>
    {
        public MvxEntryElementValueBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var entryElement = this.View;
            if (entryElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - entryElement is null in MvxEntryElementValueBinding");
            }
            else
            {
                entryElement.Changed += this.EntryElementOnChanged;
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        private void EntryElementOnChanged(object sender, EventArgs eventArgs)
        {
            this.FireValueChanged(this.View.Value);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var entryElement = this.View;
                if (entryElement != null)
                {
                    entryElement.Changed -= this.EntryElementOnChanged;
                }
            }
        }
    }
}