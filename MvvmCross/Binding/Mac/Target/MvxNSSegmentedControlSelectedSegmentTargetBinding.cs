#if __UNIFIED__
using AppKit;
using Foundation;
using ObjCRuntime;
#else
#endif

namespace MvvmCross.Binding.Mac.Target
{
    using System.Reflection;

    using global::MvvmCross.Platform.Platform;

    using MvvmCross.Binding.Bindings.Target;

    public class MvxNSSegmentedControlSelectedSegmentTargetBinding : MvxPropertyInfoTargetBinding<NSSegmentedControl>
    {
        private bool _subscribed;

        public MvxNSSegmentedControlSelectedSegmentTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        private void HandleValueChanged(object sender, System.EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged((int)view.SelectedSegment);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override void SubscribeToEvents()
        {
            var segmentedControl = View;
            if (segmentedControl == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSSegmentedControl is null in MvxNSSegmentedControlSelectedSegmentTargetBinding");
                return;
            }

            this._subscribed = true;
            segmentedControl.Activated += HandleValueChanged;
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as NSSegmentedControl;
            if (view == null)
                return;

            view.SelectedSegment = (int)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null && this._subscribed)
                {
                    view.Activated -= HandleValueChanged;
                    this._subscribed = false;
                }
            }
        }
    }
}