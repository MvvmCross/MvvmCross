// MvxTextFocusTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxTextViewFocusTargetBinding
        : MvxAndroidTargetBinding
    {
        private IDisposable _subscription;

        protected EditText TextField => Target as EditText;

        public override Type TargetType => typeof(string);

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