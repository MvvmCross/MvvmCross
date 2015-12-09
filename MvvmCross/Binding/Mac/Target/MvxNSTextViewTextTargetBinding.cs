// MvxUITextViewTextTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
#else
#endif

namespace MvvmCross.Binding.Mac.Target
{
    using System;
    using System.Reflection;

    using global::MvvmCross.Platform.Platform;

    public class MvxNSTextViewTextTargetBinding : MvxPropertyInfoTargetBinding<NSTextView>
    {
        public MvxNSTextViewTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var editText = View;
            if (editText == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - NSTextView is null in MvxNSTextViewTextTargetBinding");
            }
            else
            {
                // Todo: Perhaps we want to trigger on editing complete rather than didChange
                editText.TextDidChange += EditTextDidChange;
            }
        }

        private void EditTextDidChange(object sender, EventArgs eventArgs)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.TextStorage.ToString());
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void SetValueImpl(object target, object value)
        {
            base.SetValueImpl(target, value ?? "");
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null)
                {
                    editText.TextDidChange -= EditTextDidChange;
                }
            }
        }
    }
}