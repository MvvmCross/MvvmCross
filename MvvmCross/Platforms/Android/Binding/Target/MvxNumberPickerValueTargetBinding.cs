using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxNumberPickerValueTargetBinding : MvxPropertyInfoTargetBinding<NumberPicker>
    {
        public MvxNumberPickerValueTargetBinding(object target, PropertyInfo targetPropertyInfo) : base(target, targetPropertyInfo)
        {
        }

        private IDisposable _subscription;

        // this variable isn't used, but including this here prevents Mono from optimising the call out!
        private int JustForReflection
        {
            get { return View.Value; }
            set { View.Value = value; }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var numberPicker = (NumberPicker)target;
            if (numberPicker == null)
                return;

            numberPicker.Value = (int)value;
        }

        private void NumberPickerValueChanged(object sender, NumberPicker.ValueChangeEventArgs e)
        {
            if (!e.OldVal.Equals(e.NewVal))
                FireValueChanged(e.NewVal);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var numberPicker = View;
            if (numberPicker == null)
            {
                MvxBindingLog.Error("Error - NumberPicker is null in MvxNumberPickerValueTargetBinding");
                return;
            }

            _subscription = numberPicker.WeakSubscribe<NumberPicker, NumberPicker.ValueChangeEventArgs>(
                nameof(numberPicker.ValueChanged),
                NumberPickerValueChanged);
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
