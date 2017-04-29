// MvxUISwitchOnTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using AppKit;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using ObjCRuntime;

namespace MvvmCross.Mac.Views.Presenters
{
    public class MvxNSSwitchOnTargetBinding : MvxPropertyInfoTargetBinding<NSButton>
    {
        public MvxNSSwitchOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var checkBox = View;
            if (checkBox == null)
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSButton is null in MvxNSSwitchOnTargetBinding");
            else
                checkBox.Action = new Selector("checkBoxAction:");
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        [Export("checkBoxAction:")]
        private void checkBoxAction()
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.State == NSCellStateValue.On);
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