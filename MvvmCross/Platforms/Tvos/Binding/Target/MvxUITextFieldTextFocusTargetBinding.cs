// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    public class MvxUITextFieldTextFocusTargetBinding : MvxTargetBinding
    {
        private bool _subscribed;

        protected UITextField TextField => Target as UITextField;

        public override Type TargetValueType => typeof(string);

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
