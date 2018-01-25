using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUISegmentedControlSelectedSegmentTargetBinding : MvxPropertyInfoTargetBinding<UISegmentedControl>
    {
        private IDisposable _subscription;

        public MvxUISegmentedControlSelectedSegmentTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var segmentedControl = View;
            if (segmentedControl == null)
            {
                MvxBindingTrace.Error( "Error - UISegmentedControl is null in MvxUISegmentedControlSelectedSegmentTargetBinding");
                return;
            }

            _subscription = segmentedControl.WeakSubscribe(nameof(segmentedControl.ValueChanged), HandleValueChanged);
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UISegmentedControl;
            if (view == null) return;

            view.SelectedSegment = (nint)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged((int)view.SelectedSegment);
        }
    }
}