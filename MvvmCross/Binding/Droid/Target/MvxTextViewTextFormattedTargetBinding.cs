// MvxTextViewTextFormattedTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Text;
    using Android.Widget;

    using MvvmCross.Platform.Platform;

    public class MvxTextViewTextFormattedTargetBinding
        : MvxAndroidTargetBinding
        , IMvxEditableTextView
    {
        private readonly bool _isEditTextBinding;
        private bool _subscribed;

        protected TextView TextView => Target as TextView;

        public MvxTextViewTextFormattedTargetBinding(TextView target)
            : base(target)
        {
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - TextView is null in MvxTextViewTextFormattedTargetBinding");
                return;
            }

            this._isEditTextBinding = target is EditText;
        }

        public override Type TargetType => typeof(ISpanned);

        protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object value)
        {
            if (!this._isEditTextBinding)
                return false;

            return this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);
        }

        protected override void SetValueImpl(object target, object toSet)
        {
            ((TextView)target).TextFormatted = (ISpanned)toSet;
        }

        public override MvxBindingMode DefaultMode => this._isEditTextBinding ? MvxBindingMode.TwoWay : MvxBindingMode.OneWay;

        public override void SubscribeToEvents()
        {
            var view = this.TextView;
            if (view == null)
                return;

            view.AfterTextChanged += this.EditTextOnAfterTextChanged;
            this._subscribed = true;
        }

        private void EditTextOnAfterTextChanged(object sender, AfterTextChangedEventArgs afterTextChangedEventArgs)
        {
            FireValueChanged(this.TextView.TextFormatted);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                if (this._isEditTextBinding)
                {
                    var editText = this.TextView;
                    if (editText != null && this._subscribed)
                    {
                        editText.AfterTextChanged -= this.EditTextOnAfterTextChanged;
                        this._subscribed = false;
                    }
                }
            }
        }

        public string CurrentText
        {
            get
            {
                var view = this.TextView;
                return view?.TextFormatted.ToString();
            }
        }
    }
}