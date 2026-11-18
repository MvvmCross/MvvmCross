// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSTextViewTextTargetBinding : MvxPropertyInfoTargetBinding<NSTextView>
    {
        private readonly IDisposable? _subscription;

        public MvxNSTextViewTextTargetBinding(NSTextView target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var editText = View;
            if (editText == null)
            {
                MvxBindingLog.Instance?.LogError(
                                      "NSTextView is null in MvxNSTextViewTextTargetBinding");
            }
            else
            {
                // Todo: Perhaps we want to trigger on editing complete rather than didChange
                _subscription = editText.WeakSubscribe(nameof(NSTextView.TextDidChange), EditTextDidChange);
            }
        }

        private void EditTextDidChange(object? sender, EventArgs eventArgs)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.Value);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void SetValueImpl(object target, object? value)
        {
            base.SetValueImpl(target, value ?? "");
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                _subscription?.Dispose();
            }
        }
    }
}
