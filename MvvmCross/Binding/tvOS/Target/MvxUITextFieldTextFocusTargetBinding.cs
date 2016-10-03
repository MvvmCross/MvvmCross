// MvxUITextFieldTextFocusTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.tvOS.Target
{
    using System;

    using MvvmCross.Binding.Bindings.Target;

    using UIKit;

    public class MvxUITextFieldTextFocusTargetBinding : MvxTargetBinding
    {
        private bool _subscribed;

        protected UITextField TextField => Target as UITextField;

        public override Type TargetType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public MvxUITextFieldTextFocusTargetBinding(object target)
            : base(target)
        {
        }

        public override void SetValue(object value)
        {
            if (this.TextField == null) return;

            value = value ?? string.Empty;
            this.TextField.Text = value.ToString();
        }

        public override void SubscribeToEvents()
        {
            if (this.TextField == null) return;

            this.TextField.EditingDidEnd += this.HandleLostFocus;
            this._subscribed = true;
        }

        private void HandleLostFocus(object sender, EventArgs e)
        {
            if (this.TextField == null) return;

            FireValueChanged(this.TextField.Text);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            if (this.TextField != null && this._subscribed)
            {
                this.TextField.EditingDidEnd -= this.HandleLostFocus;
                this._subscribed = false;
            }
        }
    }
}