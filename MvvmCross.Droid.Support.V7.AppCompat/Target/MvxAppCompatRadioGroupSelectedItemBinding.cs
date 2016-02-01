// MvxAppCompatRadioGroupSelectedItemBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System;

    using Android.Support.V7.Widget;
    using Android.Widget;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Droid.Target;
    using MvvmCross.Binding.Droid.Views;
    using MvvmCross.Droid.Support.V7.AppCompat.Widget;

    public class MvxAppCompatRadioGroupSelectedItemBinding : MvxAndroidTargetBinding
    {
        private object _currentValue;

        public MvxAppCompatRadioGroupSelectedItemBinding(MvxAppCompatRadioGroup radioGroup)
            : base(radioGroup)
        {
            radioGroup.CheckedChange += this.RadioGroupCheckedChanged;
        }

        private bool CheckValueChanged(object newValue)
        {
            bool changed;
            if (newValue == null)
            {
                changed = (this._currentValue != null);
            }
            else
            {
                changed = !(newValue.Equals(this._currentValue));
            }
            return changed;
        }

        private void RadioGroupCheckedChanged(object sender, RadioGroup.CheckedChangeEventArgs args)
        {
            var radioGroup = (MvxAppCompatRadioGroup)this.Target;
            if (radioGroup == null) { return; }

            object newValue = null;
            var r = radioGroup.FindViewById<AppCompatRadioButton>(args.CheckedId);
            var li = r?.Parent as MvxListItemView;
            if (li != null)
            {
                newValue = li.DataContext;
            }

            bool changed = this.CheckValueChanged(newValue);
            if (!changed) { return; }

            this._currentValue = newValue;
            this.FireValueChanged(newValue);
        }

        protected override void SetValueImpl(object target, object newValue)
        {
            var radioGroup = (MvxAppCompatRadioGroup)target;
            if (radioGroup == null) { return; }

            bool changed = this.CheckValueChanged(newValue);
            if (!changed) { return; }

            int checkid = Android.Views.View.NoId;

            // find the radio button associated with the new value
            if (newValue != null)
            {
                for (int i = 0; i < radioGroup.ChildCount; i++)
                {
                    var li = radioGroup.GetChildAt(i) as MvxListItemView;
                    if (li != null)
                    {
                        if (newValue.Equals(li.DataContext))
                        {
                            var radioButton = li.GetChildAt(0) as AppCompatRadioButton;
                            if (radioButton != null)
                            {
                                checkid = radioButton.Id;
                                break;
                            }
                        }
                    }
                }
            }

            if (checkid == Android.Views.View.NoId)
            {
                radioGroup.ClearCheck();
            }
            else
            {
                radioGroup.Check(checkid);
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override Type TargetType => typeof(object);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var radioGroup = (MvxAppCompatRadioGroup)this.Target;
                if (radioGroup != null)
                {
                    radioGroup.CheckedChange -= this.RadioGroupCheckedChanged;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}