// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxTextViewFocusTargetBinding
        : MvxAndroidTargetBinding
    {
        private IDisposable _subscription;

        protected EditText TextField => Target as EditText;

        public override Type TargetValueType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public MvxTextViewFocusTargetBinding(object target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (TextField == null) return;

            value = value ?? string.Empty;
            TextField.Text = value.ToString();
        }

        public override void SubscribeToEvents()
        {
            if (TextField == null) return;

            _subscription = TextField.WeakSubscribe<View, View.FocusChangeEventArgs>(
                nameof(TextField.FocusChange),
                HandleLostFocus);
        }

        private void HandleLostFocus(object sender, View.FocusChangeEventArgs e)
        {
            if (TextField == null) return;

            if (!e.HasFocus)
                FireValueChanged(TextField.Text);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}
