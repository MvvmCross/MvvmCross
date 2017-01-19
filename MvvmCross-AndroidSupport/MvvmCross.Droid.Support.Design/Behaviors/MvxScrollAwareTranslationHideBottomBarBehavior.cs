using System;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using MvvmCross.Droid.Support.Design.Extensions;
using Object = Java.Lang.Object;

namespace MvvmCross.Droid.Support.Design.Behaviors
{
    [Register("mvvmcross.droid.support.design.behaviors.MvxScrollAwareTranslationAutoHideBehavior")]
    public class MvxScrollAwareTranslationHideBottomBarBehavior : CoordinatorLayout.Behavior
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
        {
        }

        public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Object child,
            View directTargetChild, View target, int nestedScrollAxes)
        {
            return nestedScrollAxes == ViewCompat.ScrollAxisVertical ||
                   base.OnStartNestedScroll(coordinatorLayout, child, directTargetChild, target, nestedScrollAxes);
        }

        public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Object child, View target,
            int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
        {
            base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);

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
