// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSTextViewTextTargetBinding : MvxConvertingTargetBinding<NSTextView, string>
    {
        public MvxNSTextViewTextTargetBinding(NSTextView target)
            : base(target)
        {
            var editText = Target;
            if (editText == null)
            {
                MvxBindingLog.Instance?.LogError(
                                      "NSTextView is null in MvxNSTextViewTextTargetBinding");
            }
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            // Todo: Perhaps we want to trigger on editing complete rather than didChange
            if (Target is { } editText)
                editText.TextDidChange += EditTextDidChange;
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

        protected override void SetValueImpl(NSTextView target, string value)
        {
            target?.TextStorage.SetString(new NSAttributedString(value ?? string.Empty));
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
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
