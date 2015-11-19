// MvxUISwitchOnTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
    public class MvxNSSwitchOnTargetBinding : MvxPropertyInfoTargetBinding<NSButton>
    {
        public MvxNSSwitchOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var checkBox = View;
            if (checkBox == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSButton is null in MvxNSSwitchOnTargetBinding");
            }
            else
            {
                checkBox.Action = new MonoMac.ObjCRuntime.Selector("checkBoxAction:");
            }
        }

        [Export("checkBoxAction:")]
        private void checkBoxAction()
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.State == NSCellStateValue.On);
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
                    //                    view.ValueChanged -= HandleValueChanged;
                }
            }
        }
    }
}