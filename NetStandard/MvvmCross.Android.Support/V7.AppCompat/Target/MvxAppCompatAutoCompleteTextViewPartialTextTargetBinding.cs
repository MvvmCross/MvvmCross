﻿// MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.WeakSubscription;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    public class MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding
       : MvxAndroidPropertyInfoTargetBinding<MvxAppCompatAutoCompleteTextView>
    {
        protected IMvxLog Log = Mvx.Resolve<IMvxLogProvider>().GetLogFor<MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding>();
        private IDisposable _subscription;

        public MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                Log.Trace("Error - autoComplete is null in MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding");
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
