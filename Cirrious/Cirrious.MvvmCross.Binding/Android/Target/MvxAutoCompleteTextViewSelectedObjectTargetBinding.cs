using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Android.Views;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Android.Target
{
    public class MvxAutoCompleteTextViewSelectedObjectTargetBinding : MvxPropertyInfoTargetBinding<MvxBindableAutoCompleteTextView>
    {
        public MvxAutoCompleteTextViewSelectedObjectTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - autoComplete is null in MvxAutoCompleteTextViewSelectedObjectTargetBinding");
            }
            else
            {
                autoComplete.SelectedObjectChanged += AutoCompleteOnSelectedObjectChanged;
            }
        }

        private void AutoCompleteOnSelectedObjectChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.SelectedObject);
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
                    autoComplete.PartialTextChanged -= AutoCompleteOnSelectedObjectChanged;
                }
            }
        }
    }
}