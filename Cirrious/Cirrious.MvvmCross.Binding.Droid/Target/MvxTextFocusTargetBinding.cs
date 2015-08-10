// MvxTextFocusTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Target;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxTextFocusTargetBinding
        : MvxAndroidTargetBinding
    {
        private bool _subscribed;

        protected EditText TextField
        {
            get { return Target as EditText; }
        }

        public override Type TargetType
        {
            get { return typeof (string); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public FocusTextBinding(object target)
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
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            if (TextField != null && _subscribed) {
                TextField.FocusChange -= HandleLostFocus;
                _subscribed = false;
            }
        }
    }
}