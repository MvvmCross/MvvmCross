// MvxUISwitchOnTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace MvvmCross.Binding.Mac.Target
{
    using System.Reflection;

    using global::MvvmCross.Platform.Platform;

    using MvvmCross.Binding.Bindings.Target;

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
                checkBox.Activated += HandleCheckBoxAction;
            }
        }

        private void HandleCheckBoxAction(object sender, System.EventArgs e)
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
                    view.Activated -= HandleCheckBoxAction;
                }
            }
        }
    }
}