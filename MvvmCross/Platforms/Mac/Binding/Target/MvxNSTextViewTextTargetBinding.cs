// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using Foundation;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSTextViewTextTargetBinding : MvxTargetBinding<NSTextView, string>
    {
        public MvxNSTextViewTextTargetBinding(NSTextView target)
            : base(target)
        {
            if (Target == null)
            {
                MvxBindingLog.Instance?.LogError(
                                      "NSTextView is null in MvxNSTextViewTextTargetBinding");
            }
        }

        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            if (Target is { } editText)
            {
                // Todo: Perhaps we want to trigger on editing complete rather than didChange
                editText.TextDidChange += EditTextDidChange;
            }
        }

        private void EditTextDidChange(object sender, EventArgs eventArgs)
        {
            var view = Target;
            if (view == null)
                return;
            FireValueChanged(view.TextStorage.Value);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void SetValue(string value)
        {
            if (Target is not { } editView)
                return;

            // TextStorage.SetString will move caret to the end. To preserve the caret position, save and restore it later.
            var selectedRange = editView.SelectedRange;

            editView.TextStorage.SetString(new NSAttributedString(value ?? string.Empty));

            var textLength = editView.TextStorage.Length;
            if (selectedRange.Location <= textLength)
            {
                var length = Math.Min(selectedRange.Length, textLength - selectedRange.Location);
                editView.SetSelectedRange(new NSRange(selectedRange.Location, length));
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = Target;
                if (editText != null)
                {
                    editText.TextDidChange -= EditTextDidChange;
                }
            }
        }
    }
}
