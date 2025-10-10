// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSSwitchOnTargetBinding : MvxPropertyInfoTargetBinding<NSButton>
    {
        public MvxNSSwitchOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var checkBox = View;
            if (checkBox == null)
            {
                MvxBindingLog.Instance?.LogError("NSButton is null in MvxNSSwitchOnTargetBinding");
            }
            else
            {
                checkBox.Activated += HandleButtonCheckBoxAction;
            }
        }

        private void HandleButtonCheckBoxAction(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.State == NSCellStateValue.On);
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
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

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null)
                {
                    view.Activated -= HandleButtonCheckBoxAction;
                }
            }
        }
    }
}
