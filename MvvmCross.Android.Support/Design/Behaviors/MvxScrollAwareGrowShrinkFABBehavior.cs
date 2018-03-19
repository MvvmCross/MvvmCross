// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Object = Java.Lang.Object;

namespace MvvmCross.Droid.Support.Design.Behaviors
{
    [Register("mvvmcross.droid.support.design.behaviors.MvxScrollAwareGrowShrinkFABBehavior")]
    public class MvxScrollAwareGrowShrinkFABBehavior 
        : CoordinatorLayout.Behavior
    {
        public MvxScrollAwareGrowShrinkFABBehavior(IntPtr handle, JniHandleOwnership ownership) 
            : base(handle, ownership)
        {
        }

        public MvxScrollAwareGrowShrinkFABBehavior()
        {
        }

        public MvxScrollAwareGrowShrinkFABBehavior(Context context, IAttributeSet attributeSet) 
            : base(context, attributeSet)
        {
        }

        public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Object child, View directTargetChild, View target, int axes, int type)
        {
            return axes == ViewCompat.ScrollAxisVertical ||
                base.OnStartNestedScroll(coordinatorLayout, child, directTargetChild, target, axes, type);
        }

        public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed, int type)
        {
            base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed, type);

            var floatingActionButtonChild = child.JavaCast<FloatingActionButton>();

            if (dyConsumed > 0 && floatingActionButtonChild.Visibility == ViewStates.Visible)
                floatingActionButtonChild.Hide();
            else if (dyConsumed < 0 && floatingActionButtonChild.Visibility != ViewStates.Visible)
                floatingActionButtonChild.Show();
        }    
    }
}
