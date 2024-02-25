// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    public class MvxUITextViewTextTargetBinding
        : MvxConvertingTargetBinding
    {
        protected UITextView View => Target as UITextView;

        private bool _subscribed;

        public MvxUITextViewTextTargetBinding(UITextView target)
            : base(target)
        {
        }

        private void EditTextOnChanged(object sender, EventArgs eventArgs)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.Text);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = View;
            if (target == null)
            {
                MvxBindingLog.Instance?.LogError("UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

            target.Changed += EditTextOnChanged;
            _subscribed = true;
        }

        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UITextView)target;
            if (view == null)
                return;

            view.Text = (string)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null && _subscribed)
                {
                    editText.Changed -= EditTextOnChanged;
                    _subscribed = false;
                }
            }
        }
    }
}
