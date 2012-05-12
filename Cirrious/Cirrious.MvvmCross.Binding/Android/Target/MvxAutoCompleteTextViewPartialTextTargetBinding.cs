using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Android.Views;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Android.Target
{
    public class MvxAutoCompleteTextViewPartialTextTargetBinding : MvxPropertyInfoTargetBinding<MvxBindableAutoCompleteTextView>
    {
#warning Cut and paste could be reduced for these controls - could also consider using attributes!
        public MvxAutoCompleteTextViewPartialTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - autoComplete is null in MvxAutoCompleteTextViewPartialTextTargetBinding");
            }
            else
            {
                autoComplete.PartialTextChanged += AutoCompleteOnPartialTextChanged;
            }
        }

        private void AutoCompleteOnPartialTextChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.PartialText);
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.OneWayToSource;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var autoComplete = View;
                if (autoComplete != null)
                {
                    autoComplete.PartialTextChanged -= AutoCompleteOnPartialTextChanged;
                }
            }
        }
    }
}