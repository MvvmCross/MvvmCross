﻿// MvxUIStepperValueTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIPageControlCurrentPageTargetBinding : MvxPropertyInfoTargetBinding<UIPageControl>
    {
        private IDisposable _subscription;

        public MvxUIPageControlCurrentPageTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UIPageControl;
            if (view == null) return;

            view.CurrentPage = (nint)value;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.CurrentPage);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var pageControl = View;
            if (pageControl == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "UIPageControl is null in MvxUIPageControlCurrentPageTargetBinding");
                return;
            }

            _subscription = pageControl.WeakSubscribe(nameof(pageControl.ValueChanged), HandleValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}