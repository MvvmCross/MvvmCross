namespace MvvmCross.Binding.iOS.Target
{
    using System;
    using System.Reflection;

    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class MvxUISegmentedControlSelectedSegmentTargetBinding : MvxPropertyInfoTargetBinding<UISegmentedControl>
    {
        private bool _subscribed;

        public MvxUISegmentedControlSelectedSegmentTargetBinding(object target, PropertyInfo targetPropertyInfo)
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

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var segmentedControl = View;
            if (segmentedControl == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UISegmentedControl is null in MvxUISegmentedControlSelectedSegmentTargetBinding");
                return;
            }

            this._subscribed = true;
            segmentedControl.ValueChanged += HandleValueChanged;
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UISegmentedControl;
            if (view == null)
                return;

            view.SelectedSegment = (nint)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null && this._subscribed)
                {
                    view.ValueChanged -= HandleValueChanged;
                    this._subscribed = false;
                }
            }
        }
    }
}