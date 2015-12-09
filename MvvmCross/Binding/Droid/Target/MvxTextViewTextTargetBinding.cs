// MvxTextViewTextTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Text;
using Android.Widget;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using System;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxTextViewTextTargetBinding
        : MvxAndroidTargetBinding
        , IMvxEditableTextView
    {
        private readonly bool _isEditTextBinding;
        private bool _subscribed;

        protected TextView TextView => Target as TextView;

        public MvxTextViewTextTargetBinding(TextView target)
            : base(target)
        {
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - TextView is null in MvxTextViewTextTargetBinding");
                return;
            }

            _isEditTextBinding = target is EditText;
        }

        public override Type TargetType => typeof(string);

        protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object value)
        {
            if (!_isEditTextBinding)
                return false;

            return this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);
        }

        protected override void SetValueImpl(object target, object toSet)
        {
            ((TextView)target).Text = (string)toSet;
        }

        public override MvxBindingMode DefaultMode => _isEditTextBinding ? MvxBindingMode.TwoWay : MvxBindingMode.OneWay;

        public override void SubscribeToEvents()
        {
            var view = TextView;
            if (view == null)
                return;

            view.AfterTextChanged += EditTextOnAfterTextChanged;
            _subscribed = true;
        }

        private void EditTextOnAfterTextChanged(object sender, AfterTextChangedEventArgs afterTextChangedEventArgs)
        {
            FireValueChanged(TextView.Text);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                if (_isEditTextBinding)
                {
                    var editText = TextView;
                    if (editText != null && _subscribed)
                    {
                        editText.AfterTextChanged -= EditTextOnAfterTextChanged;
                        _subscribed = false;
                    }
                }
            }
        }

        public string CurrentText
        {
            get
            {
                var view = TextView;
                return view?.Text;
            }
        }
    }
}