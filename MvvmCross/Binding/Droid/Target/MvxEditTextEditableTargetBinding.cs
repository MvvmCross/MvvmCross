// MvxEditTextEditableTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Thom Ritterfeld @tritter

using System;
using Android.Text;
using Android.Widget;
using Android.Views;
using MvvmCross.Binding.ExtensionMethods;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxEditTextEditableTargetBinding
        : MvxAndroidTargetBinding
    {
		public MvxEditTextEditableTargetBinding(EditText view)
			: base(view){}

		protected EditText TextField => Target as EditText;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(bool);

        protected override void SetValueImpl(object target, object value)
        {
			if (TextField == null) return;
			TextField.Clickable = value.ConvertToBoolean();
			TextField.FocusableInTouchMode = value.ConvertToBoolean();
			TextField.Focusable = value.ConvertToBoolean();

        }
    }
}
