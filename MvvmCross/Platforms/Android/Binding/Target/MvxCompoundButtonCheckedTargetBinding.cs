// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxCompoundButtonCheckedTargetBinding
        : MvxAndroidPropertyInfoTargetBinding<CompoundButton>
    {
        private IDisposable _subscription;

        public MvxCompoundButtonCheckedTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var compoundButton = View;
            if (compoundButton == null)
            {
                MvxBindingLog.Error(
                                      "Error - compoundButton is null in MvxCompoundButtonCheckedTargetBinding");
                return;
            }

            _subscription = compoundButton.WeakSubscribe<CompoundButton, CompoundButton.CheckedChangeEventArgs>(
                nameof(compoundButton.CheckedChange),
                CompoundButtonOnCheckedChange);
        }

        private void CompoundButtonOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs args)
        {
            FireValueChanged(View.Checked);
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
