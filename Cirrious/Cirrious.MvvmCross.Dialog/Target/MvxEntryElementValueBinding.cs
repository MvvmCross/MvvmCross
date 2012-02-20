using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using MonoTouch.Dialog;

namespace Cirrious.MvvmCross.Dialog.Touch.Target
{
    public class MvxEntryElementValueBinding: MvxPropertyInfoTargetBinding<EntryElement>
    {
        public MvxEntryElementValueBinding(object target, PropertyInfo targetPropertyInfo) 
            : base(target, targetPropertyInfo)
        {
            var entryElement = View;
            if (entryElement == null)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Error, "Error - entryElement is null in MvxEntryElementValueBinding");
            }
            else
            {
                entryElement.Changed += EntryElementOnChanged;
            }
        }

        private void EntryElementOnChanged(object sender, EventArgs eventArgs)
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
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var entryElement = View;
                if (entryElement != null)
                {
                    entryElement.Changed -= EntryElementOnChanged;
                }
            }
        }
    }
}
