﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AndroidX.AppCompat.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.WeakSubscription;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxAppCompatRadioGroupSelectedItemBinding
        : MvxAndroidTargetBinding
    {
        private object _currentValue;
        private IDisposable _subscription;

        public MvxAppCompatRadioGroupSelectedItemBinding(MvxAppCompatRadioGroup radioGroup)
            : base(radioGroup)
        {
            _subscription = radioGroup.WeakSubscribe<RadioGroup, RadioGroup.CheckedChangeEventArgs>(
                nameof(RadioGroup.CheckedChange),
                RadioGroupCheckedChanged);
        }

        private bool CheckValueChanged(object newValue)
        {
            bool changed;
            if (newValue == null)
            {
                changed = _currentValue != null;
            }
            else
            {
                changed = !newValue.Equals(_currentValue);
            }
            return changed;
        }

        private void RadioGroupCheckedChanged(object sender, RadioGroup.CheckedChangeEventArgs args)
        {
            var radioGroup = (MvxAppCompatRadioGroup)Target;
            if (radioGroup == null)
                return;

            object newValue = null;

            var r = radioGroup.FindViewById<AppCompatRadioButton>(args.CheckedId);
            if (r != null)
            {
                var index = radioGroup.IndexOfChild(r);
                newValue = radioGroup.Adapter.GetRawItem(index);
            }

            bool changed = CheckValueChanged(newValue);
            if (!changed)
                return;

            _currentValue = newValue;
            FireValueChanged(newValue);
        }

        protected override void SetValueImpl(object target, object newValue)
        {
            var radioGroup = (MvxAppCompatRadioGroup)target;
            if (radioGroup == null)
                return;

            bool changed = CheckValueChanged(newValue);
            if (!changed)
                return;

            int checkid = View.NoId;

            // find the radio button associated with the new value
            if (newValue != null)
            {
                for (int i = 0; i < radioGroup.ChildCount; i++)
                {
                    var li = radioGroup.GetChildAt(i);
                    var data = radioGroup.Adapter.GetRawItem(i);
                    if (li != null)
                    {
                        if (newValue.Equals(data))
                        {
                            var radioButton = li as AppCompatRadioButton;
                            if (radioButton != null)
                            {
                                checkid = radioButton.Id;
                                break;
                            }
                        }
                    }
                }
            }

            if (checkid == View.NoId)
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
                _subscription?.Dispose();
                _subscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}
