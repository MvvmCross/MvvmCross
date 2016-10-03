// MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.Droid.Target;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System;
    using System.Reflection;

    using MvvmCross.Binding;
    using MvvmCross.Droid.Support.V7.AppCompat.Widget;
    using MvvmCross.Platform.Platform;

    public class MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding
       : MvxAndroidPropertyInfoTargetBinding<MvxAppCompatAutoCompleteTextView>
    {
        private IDisposable _subscription;

        public MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = this.View;
            if (autoComplete == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - autoComplete is null in MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding");
            }
        }

        private void AutoCompleteOnPartialTextChanged(object sender, EventArgs eventArgs)
        {
            this.FireValueChanged(this.View.PartialText);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWayToSource;

        public override void SubscribeToEvents()
        {
            var autoComplete = this.View;
            if (autoComplete == null)
                return;

            _subscription = autoComplete.WeakSubscribe(
                nameof(autoComplete.PartialTextChanged),
                AutoCompleteOnPartialTextChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
            }
            base.Dispose(isDisposing);
        }
    }
}