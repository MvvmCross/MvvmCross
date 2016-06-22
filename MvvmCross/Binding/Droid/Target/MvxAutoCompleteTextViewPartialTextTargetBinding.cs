// MvxAutoCompleteTextViewPartialTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;

using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxAutoCompleteTextViewPartialTextTargetBinding
       : MvxPropertyInfoTargetBinding<MvxAutoCompleteTextView>
    {
        private bool _subscribed;

        public MvxAutoCompleteTextViewPartialTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - autoComplete is null in MvxAutoCompleteTextViewPartialTextTargetBinding");
            }
        }

        private void AutoCompleteOnPartialTextChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.PartialText);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWayToSource;

        public override void SubscribeToEvents()
        {
            var autoComplete = View;
            if (autoComplete == null)
                return;

            _subscribed = true;
            autoComplete.PartialTextChanged += AutoCompleteOnPartialTextChanged;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var autoComplete = View;
                if (autoComplete != null && _subscribed)
                {
                    autoComplete.PartialTextChanged -= AutoCompleteOnPartialTextChanged;
                    _subscribed = false;
                }
            }

            base.Dispose(isDisposing);
        }
    }
}