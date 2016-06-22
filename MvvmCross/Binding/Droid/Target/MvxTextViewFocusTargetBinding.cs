// MvxTextFocusTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxTextViewFocusTargetBinding
        : MvxConvertingTargetBinding
    {
        private bool _subscribed;

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

            TextField.FocusChange += HandleLostFocus;
            _subscribed = true;
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
                if (TextField != null && _subscribed)
                {
                    TextField.FocusChange -= HandleLostFocus;
                    _subscribed = false;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}