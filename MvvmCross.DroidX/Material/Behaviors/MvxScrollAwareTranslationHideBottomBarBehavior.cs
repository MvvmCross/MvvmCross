// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.View;
using MvvmCross.DroidX.Material.Extensions;
using Object = Java.Lang.Object;

namespace MvvmCross.DroidX.Material.Behaviors
{
    [Register("mvvmcross.droidx.material.behaviors.MvxScrollAwareTranslationAutoHideBehavior")]
    public class MvxScrollAwareTranslationHideBottomBarBehavior
        : CoordinatorLayout.Behavior
    {
        private static readonly float MinimalScrollDistance = 25;
        private bool isBottomBarVisible = true;
        private float scrolledDistance;

        public MvxScrollAwareTranslationHideBottomBarBehavior(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public MvxScrollAwareTranslationHideBottomBarBehavior()
        {
        }

        public MvxScrollAwareTranslationHideBottomBarBehavior(Context context, IAttributeSet attributeSet)
            : base(context, attributeSet)
        {
        }

        public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Object child,
            View directTargetChild, View target, int axes, int type)
        {
            return axes == ViewCompat.ScrollAxisVertical ||
                   base.OnStartNestedScroll(coordinatorLayout, child, directTargetChild, target, axes, type);
        }

        public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Object child, View target,
            int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed, int type)
        {
            base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed, type);

            var viewChild = child.JavaCast<View>();

            if (!viewChild.Enabled)
                return;

            if (isBottomBarVisible && scrolledDistance >= MinimalScrollDistance)
            {
                viewChild.HideWithTranslateAnimation();
                scrolledDistance = 0;
                isBottomBarVisible = false;
            }
            else if (!isBottomBarVisible && scrolledDistance <= -MinimalScrollDistance)
            {
                viewChild.ShowWithTranslateAnimation();
                scrolledDistance = 0;
                isBottomBarVisible = true;
            }

            if ((isBottomBarVisible && dyConsumed >= 0) || (!isBottomBarVisible && dyConsumed <= 0))
                scrolledDistance += dyConsumed;
        }
    }
}
