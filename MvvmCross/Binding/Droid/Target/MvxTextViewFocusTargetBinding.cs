// MvxTextFocusTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Views;
    using Android.Widget;

    public class MvxTextViewFocusTargetBinding
        : MvxAndroidTargetBinding
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
            if (this.TextField == null) return;

            value = value ?? string.Empty;
            this.TextField.Text = value.ToString();
        }

        public override void SubscribeToEvents()
        {
            if (this.TextField == null) return;

            this.TextField.FocusChange += this.HandleLostFocus;
            this._subscribed = true;
        }

        private void HandleLostFocus(object sender, View.FocusChangeEventArgs e)
        {
            if (this.TextField == null) return;

            if (!e.HasFocus)
                FireValueChanged(this.TextField.Text);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (this.TextField != null && this._subscribed)
                {
                    this.TextField.FocusChange -= this.HandleLostFocus;
                    this._subscribed = false;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}