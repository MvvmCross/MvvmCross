// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using AppKit;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSMenuItemOnTargetBinding : MvxPropertyInfoTargetBinding<NSMenuItem>
    {
        public MvxNSMenuItemOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var checkBox = View;
            if (checkBox == null)
            {
                MvxBindingLog.Error("Error - NSButton is null in MvxNSSwitchOnTargetBinding");
            }
            else
            {
                checkBox.Activated += HandleMenuItemCheckBoxAction;
            }
        }

        private void HandleMenuItemCheckBoxAction(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;

            FireValueChanged(view.State == NSCellStateValue.On);
        }

        protected override object MakeSafeValue(object value)
        {
            if (value is bool)
            {
                if ((bool)value)
                {
                    return (NSCellStateValue.On);
                }
                else
                {
                    return (NSCellStateValue.Off);
                }
            }
            return base.MakeSafeValue(value);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null)
                {
                    view.Activated -= HandleMenuItemCheckBoxAction;
                }
            }
        }
    }
}
