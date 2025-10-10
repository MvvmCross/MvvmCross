// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    public class MvxUITextFieldTextFocusTargetBinding : MvxTargetBinding
    {
        private bool _subscribed;

        protected UITextField TextField => Target as UITextField;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public MvxUITextFieldTextFocusTargetBinding(object target)
            : base(target)
        {
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public override void SetValue(object value)
        {
            if (TextField == null) return;

            value = value ?? string.Empty;
            TextField.Text = value.ToString();
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
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

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_subscribed)
                {
                    var textField = TextField;
                    if (textField != null)
                    {
                        textField.EditingDidEnd -= HandleLostFocus;
                    }
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
