// MvxUISwitchOnTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Mac.Views.Presenters
{
    using System.Reflection;

    using AppKit;
    using Foundation;

    using global::MvvmCross.Binding;
    using global::MvvmCross.Binding.Bindings.Target;
    using global::MvvmCross.Platform.Platform;

    public class MvxNSSwitchOnTargetBinding : MvxPropertyInfoTargetBinding<NSButton>
    {
        public MvxNSSwitchOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var checkBox = this.View;
            if (checkBox == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSButton is null in MvxNSSwitchOnTargetBinding");
            }
            else
            {
                checkBox.Action = new ObjCRuntime.Selector("checkBoxAction:");
            }
        }

        [Export("checkBoxAction:")]
        private void checkBoxAction()
        {
            var view = this.View;
            if (view == null)
                return;
            this.FireValueChanged(view.State == NSCellStateValue.On);
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
                var view = this.View;
                if (view != null)
                {
                    //                    view.ValueChanged -= HandleValueChanged;
                }
            }
        }
    }
}