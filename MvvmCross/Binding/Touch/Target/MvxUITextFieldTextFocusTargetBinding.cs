// MvxUITextFieldTextFocusTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Target;
using System;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
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
            if (TextField == null) return;

            value = value ?? string.Empty;
            TextField.Text = value.ToString();
        }

        public override void SubscribeToEvents()
        {
            if (TextField == null) return;

            TextField.EditingDidEnd += HandleLostFocus;
            _subscribed = true;
        }

        private void HandleLostFocus(object sender, EventArgs e)
        {
            if (TextField == null) return;

            FireValueChanged(TextField.Text);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            if (TextField != null && _subscribed)
            {
                TextField.EditingDidEnd -= HandleLostFocus;
                _subscribed = false;
            }
        }
    }
}