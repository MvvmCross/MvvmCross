// MvxUITextFieldTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Target
{
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Binding.ExtensionMethods;
    using MvvmCross.Platform.Platform;

    using UIKit;

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

        private void HandleEditTextValueChanged(object sender, System.EventArgs e)
        {
            var view = this.View;
            if (view == null)
                return;
            FireValueChanged(view.Text);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = this.View;
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - UITextField is null in MvxUITextFieldTextTargetBinding");
                return;
            }

            target.EditingChanged += this.HandleEditTextValueChanged;
            this._subscribed = true;
        }

        public override System.Type TargetType => typeof(string);

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
                var editText = this.View;
                if (editText != null && this._subscribed)
                {
                    editText.EditingChanged -= this.HandleEditTextValueChanged;
                    this._subscribed = false;
                }
            }
        }

        public string CurrentText
        {
            get
            {
                var view = this.View;
                return view?.Text;
            }
        }
    }
}