﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxAppCompatAutoCompleteTextViewSelectedObjectTargetBinding
        : MvxAndroidPropertyInfoTargetBinding<MvxAppCompatAutoCompleteTextView>
    {
        private IDisposable _subscription;

        public MvxAppCompatAutoCompleteTextViewSelectedObjectTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = this.View;
            if (autoComplete == null)
            {
                MvxBindingLog.Error(
                    "Error - autoComplete is null in MvxAppCompatAutoCompleteTextViewSelectedObjectTargetBinding");
            }
        }

        private void AutoCompleteOnSelectedObjectChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.SelectedObject);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWayToSource;

        public override void SubscribeToEvents()
        {
            var autoComplete = this.View;

            if (autoComplete == null)
                return;

            _subscription = autoComplete.WeakSubscribe(
                nameof(autoComplete.SelectedObjectChanged),
                AutoCompleteOnSelectedObjectChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
            }
            base.Dispose(isDisposing);
        }
    }
}
