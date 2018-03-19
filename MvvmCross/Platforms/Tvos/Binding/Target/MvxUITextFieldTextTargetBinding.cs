﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Extensions;
using UIKit;

namespace MvvmCross.Platform.Tvos.Binding.Target
{
    public class MvxUITextFieldTextTargetBinding
        : MvxConvertingTargetBinding
        , IMvxEditableTextView
    {
        protected UITextField View => Target as UITextField;

        private bool _subscribed;

        public MvxUITextFieldTextTargetBinding(UITextField target)
            : base(target)
        {
        }

        private void HandleEditTextValueChanged(object sender, EventArgs e)
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
                MvxBindingLog.Error(
                                      "Error - UITextField is null in MvxUITextFieldTextTargetBinding");
                return;
            }

            target.EditingChanged += HandleEditTextValueChanged;
            _subscribed = true;
        }

        public override Type TargetType => typeof(string);

        protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object value)
        {
            return this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UITextField)target;
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
                    editText.EditingChanged -= HandleEditTextValueChanged;
                    _subscribed = false;
                }
            }
        }

        public string CurrentText
        {
            get
            {
                var view = View;
                return view?.Text;
            }
        }
    }
}
