using System;
using System.Reflection;
using AppKit;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Mac.Target
{
    public class MvxNSPopUpButtonSelectedTagTargetBinding : MvxPropertyInfoTargetBinding<NSPopUpButton>
    {
        private bool _subscribed;

        public MvxNSPopUpButtonSelectedTagTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged((int)view.SelectedTag);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override void SubscribeToEvents()
        {
            var popupButton = View;
            if (popupButton == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSPopUpButton is null in MvxNSPopUpButtonSelectedTagTargetBinding");
                return;
            }

            _subscribed = true;
            popupButton.Activated += HandleValueChanged;
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as NSPopUpButton;
            if (view == null)
                return;
            
            view.SelectItemWithTag((int)value);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null && _subscribed)
                {
                    view.Activated -= HandleValueChanged;
                    _subscribed = false;
                }
            }
        }
    }
}