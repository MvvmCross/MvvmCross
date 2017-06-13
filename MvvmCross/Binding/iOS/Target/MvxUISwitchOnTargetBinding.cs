﻿// MvxUISwitchOnTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUISwitchOnTargetBinding
        : MvxPropertyInfoTargetBinding<UISwitch>
    {
        public MvxUISwitchOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var view = View;
            if (view == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - Switch is null in MvxUISwitchOnTargetBinding");
            }
            else
            {
                view.ValueChanged += HandleValueChanged;
            }
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.On);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null)
                {
                    view.ValueChanged -= HandleValueChanged;
                }
            }
        }
    }
}