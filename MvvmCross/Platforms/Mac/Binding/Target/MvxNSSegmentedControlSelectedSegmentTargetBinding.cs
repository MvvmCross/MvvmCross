// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using AppKit;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSSegmentedControlSelectedSegmentTargetBinding : MvxPropertyInfoTargetBinding<NSSegmentedControl>
    {
        private bool _subscribed;

        public MvxNSSegmentedControlSelectedSegmentTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        private void HandleValueChanged(object sender, EventArgs e)
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

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var segmentedControl = View;
            if (segmentedControl == null)
            {
                MvxBindingLog.Instance?.LogError("NSSegmentedControl is null in MvxNSSegmentedControlSelectedSegmentTargetBinding");
                return;
            }

            _subscribed = true;
            segmentedControl.Activated += HandleValueChanged;
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as NSSegmentedControl;
            if (view == null)
                return;

            view.SelectSegment((int)value);
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
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
