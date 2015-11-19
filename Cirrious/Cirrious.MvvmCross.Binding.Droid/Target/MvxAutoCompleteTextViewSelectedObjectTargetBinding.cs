// MvxAutoCompleteTextViewSelectedObjectTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxAutoCompleteTextViewSelectedObjectTargetBinding
        : MvxPropertyInfoTargetBinding<MvxAutoCompleteTextView>
    {
        private bool _subscribed;

        public MvxAutoCompleteTextViewSelectedObjectTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - autoComplete is null in MvxAutoCompleteTextViewSelectedObjectTargetBinding");
            }
        }

        private void AutoCompleteOnSelectedObjectChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.SelectedObject);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWayToSource;

        public override void SubscribeToEvents()
        {
            var autoComplete = View;

            if (autoComplete == null)
                return;

            _subscribed = true;
            autoComplete.SelectedObjectChanged += AutoCompleteOnSelectedObjectChanged;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var autoComplete = View;
                if (autoComplete != null && _subscribed)
                {
                    autoComplete.SelectedObjectChanged -= AutoCompleteOnSelectedObjectChanged;
                    _subscribed = false;
                }
            }
        }
    }
}