// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Extensions;

namespace MvvmCross.Platforms.Tvos.Binding.Target
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

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var target = View;
            if (target == null)
            {
                MvxBindingLog.Instance?.LogError("UITextField is null in MvxUITextFieldTextTargetBinding");
                return;
            }

            target.EditingChanged += HandleEditTextValueChanged;
            _subscribed = true;
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        [RequiresUnreferencedCode("This method uses reflection to get type information and perform conversions which may not be preserved by trimming.")]
        protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object? value)
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

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_subscribed)
                {
                    var target = View;
                    if (target != null)
                    {
                        target.EditingChanged -= HandleEditTextValueChanged;
                    }
                }
            }
            base.Dispose(isDisposing);
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
