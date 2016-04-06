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
    [Register("MvvmCross.Droid.Support.Design.Behaviors.MvxScrollAwareTranslationHideFABBehavior")]
    public class MvxScrollAwareTranslationHideFABBehavior : CoordinatorLayout.Behavior
    {
        private static readonly float MinimalScrollDistance = 25;
        private bool isFabVisible = true;
        private float scrolledDistance;

        public MvxScrollAwareTranslationHideFABBehavior(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public MvxScrollAwareTranslationHideFABBehavior()
        {
        }

        public MvxScrollAwareTranslationHideFABBehavior(Context context, IAttributeSet attributeSet)
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

            var floatingActionButtonChild = child.JavaCast<FloatingActionButton>();

            if (!floatingActionButtonChild.Enabled)
                return;

            if (isFabVisible && scrolledDistance >= MinimalScrollDistance)
            {
                floatingActionButtonChild.HideWithTranslateAnimation();
                scrolledDistance = 0;
                isFabVisible = false;
            }
            else if (!isFabVisible && scrolledDistance <= -MinimalScrollDistance)
            {
                floatingActionButtonChild.ShowWithTranslateAnimation();
                scrolledDistance = 0;
                isFabVisible = true;
            }

            if ((isFabVisible && dyConsumed >= 0) || (!isFabVisible && dyConsumed <= 0))
                scrolledDistance += dyConsumed;
        }
    }
}