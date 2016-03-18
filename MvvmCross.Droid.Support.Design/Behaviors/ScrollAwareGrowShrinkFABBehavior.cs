using System;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;

namespace MvvmCross.Droid.Support.Design.Behaviors
{
    [Register("MvvmCross.Droid.Support.Design.Behaviors.ScrollAwareGrowShrinkFABBehavior")]
    public class ScrollAwareGrowShrinkFABBehavior : CoordinatorLayout.Behavior
    {
        public ScrollAwareGrowShrinkFABBehavior(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
        {
            
        }

        public ScrollAwareGrowShrinkFABBehavior()
        {
            
        }

        public ScrollAwareGrowShrinkFABBehavior(Context context, IAttributeSet attributeSet) : base()
        {

        }

        public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View directTargetChild, View target, int nestedScrollAxes)
        {
            return nestedScrollAxes == ViewCompat.ScrollAxisVertical ||
                   base.OnStartNestedScroll(coordinatorLayout, child, directTargetChild, target, nestedScrollAxes);
        }

        public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
        {
            base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);

            var floatingActionButtonChild = child.JavaCast<FloatingActionButton>();

            if (dyConsumed > 0 && floatingActionButtonChild.Visibility == ViewStates.Visible)
                floatingActionButtonChild.Hide();
            else if (dyConsumed < 0 && floatingActionButtonChild.Visibility != ViewStates.Visible)
                floatingActionButtonChild.Show();
        }    
    }
}