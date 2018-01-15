using System;
using System.Reflection;
using AppKit;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Mac.Views;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Mac.Target
{
    public class MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding : MvxPropertyInfoTargetBinding<NSTabViewController>
    {
        private bool _subscribed;
        private NSTabViewController target;

        public MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged((int)view.SelectedTabViewItemIndex);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override void SubscribeToEvents()
        {
            var view = View;
            if (view == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSTabViewController is null in MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding");
                return;
            }

            _subscribed = true;
            if (view is MvxEventSourceTabViewController)
            {
                ((MvxEventSourceTabViewController)view).DidSelectCalled += HandleValueChanged;
            }
            else {
                try {
                    view.TabView.DidSelect += HandleValueChanged;
                }
                catch (Exception ex)
                {
                    MvxBindingTrace.Error(ex.Message);
                }
            }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as NSTabViewController;
            if (view == null)
                return;

            view.SelectedTabViewItemIndex = (int)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null && _subscribed)
                {
                    if (view is MvxEventSourceTabViewController)
                    {
                        ((MvxEventSourceTabViewController)view).DidSelectCalled -= HandleValueChanged;
                    }
                    else
                    {
                        try
                        {
                            view.TabView.DidSelect -= HandleValueChanged;
                        }
                        catch (Exception ex)
                        {
                            MvxBindingTrace.Error(ex.Message);
                        }
                    }
                    _subscribed = false;
                }
            }
        }
    }
}
