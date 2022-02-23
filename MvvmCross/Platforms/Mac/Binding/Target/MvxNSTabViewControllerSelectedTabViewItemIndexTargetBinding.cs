// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using AppKit;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platforms.Mac.Views.Base;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding : MvxPropertyInfoTargetBinding<NSTabViewController>
    {
        private bool _subscribed;

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
                MvxBindingLog.Error("Error - NSTabViewController is null in MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding");
                return;
            }

            _subscribed = true;
            if (view is MvxEventSourceTabViewController)
            {
                ((MvxEventSourceTabViewController)view).DidSelectCalled += HandleValueChanged;
            }
            else
            {
                try
                {
                    view.TabView.DidSelect += HandleValueChanged;
                }
                catch (Exception ex)
                {
                    MvxBindingLog.Error(ex.Message);
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
                            MvxBindingLog.Error(ex.Message);
                        }
                    }
                    _subscribed = false;
                }
            }
        }
    }
}
