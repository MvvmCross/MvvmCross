// MvxUISwitchOnTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using ObjCRuntime;

namespace MvvmCross.Mac.Views.Presenters
{
    using System.Reflection;

    using AppKit;
    using Foundation;

    using Binding;
    using Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

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
                checkBox.Action = new Selector("checkBoxAction:");
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