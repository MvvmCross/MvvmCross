// MvxRadioGroupSelectedItemBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Widget;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Views;
using System;


namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxRadioGroupSelectedItemBinding : MvxAndroidTargetBinding
    {
        private object _currentValue;


        public MvxRadioGroupSelectedItemBinding(MvxRadioGroup radioGroup)
            : base(radioGroup)
        {
            radioGroup.CheckedChange += RadioGroupCheckedChanged;
        }


        private bool CheckValueChanged(object newValue)
        {
            bool changed;
            if (newValue == null)
            {
                changed = (_currentValue != null);
            }
            else
            {
                changed = !(newValue.Equals(_currentValue));
            }
            return changed;
        }


        private void RadioGroupCheckedChanged(object sender, RadioGroup.CheckedChangeEventArgs args)
        {
            var radioGroup = (MvxRadioGroup)Target;
            if (radioGroup == null) { return; }

            object newValue = null;
            var r = radioGroup.FindViewById<RadioButton>(args.CheckedId);
            if (r != null)
            {
                var li = r.Parent as MvxListItemView;
                if (li != null)
                {
                    newValue = li.DataContext;
                }
            }

            bool changed = CheckValueChanged(newValue);
            if (!changed) { return; }

            _currentValue = newValue;
            FireValueChanged(newValue);
        }


        public override void SetValue(object newValue)
        {
            var radioGroup = (MvxRadioGroup)Target;
            if (radioGroup == null) { return; }

            bool changed = CheckValueChanged(newValue);
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
                            var radioButton = li.GetChildAt(0) as RadioButton;
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


        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }


        public override Type TargetType
        {
            get { return typeof(object); }
        }


        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var radioGroup = (MvxRadioGroup)Target;
                if (radioGroup != null)
                {
                    radioGroup.CheckedChange -= RadioGroupCheckedChanged;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}