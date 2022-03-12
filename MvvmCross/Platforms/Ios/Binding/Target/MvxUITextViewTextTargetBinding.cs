// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Target
{
    public class MvxUITextViewTextTargetBinding : MvxConvertingTargetBinding
    {
        private IDisposable _subscription;

        protected UITextView View => Target as UITextView;


        public MvxUITextViewTextTargetBinding(UITextView target)
            : base(target)
        {
        }

        private void EditTextOnChanged(object sender, NSTextStorageEventArgs eventArgs)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Text);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = View;
            if (target == null)
            {
                MvxBindingLog.Error(
                                      "Error - UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

            var textStorage = target.LayoutManager?.TextStorage;
            if (textStorage == null)
            {
                MvxBindingLog.Error(
                          "Error - NSTextStorage of UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

            _subscription = textStorage.WeakSubscribe<NSTextStorage, NSTextStorageEventArgs>(nameof(textStorage.DidProcessEditing), EditTextOnChanged);
        }

        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UITextView)target;
            if (view == null) return;

            view.Text = (string)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
