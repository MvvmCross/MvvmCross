// MvxUITextViewTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Target
{
    using System;

    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

    using UIKit;

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
            var view = this.View;
            if (view == null)
                return;
            FireValueChanged(view.Text);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = this.View;
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

            target.Changed += this.EditTextOnChanged;
            this._subscribed = true;
        }

        public override Type TargetType => typeof(string);

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
                var editText = this.View;
                if (editText != null && this._subscribed)
                {
                    editText.Changed -= this.EditTextOnChanged;
                    this._subscribed = false;
                }
            }
        }
    }
}