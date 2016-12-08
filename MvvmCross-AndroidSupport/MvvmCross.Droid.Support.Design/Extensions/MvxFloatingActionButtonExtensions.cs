using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.Animations;

namespace MvvmCross.Droid.Support.Design.Extensions
{
    public static class MvxFloatingActionButtonExtensions
    {
        public static FloatingActionButton ShowWithTranslateAnimation(this FloatingActionButton actionButton)
        {
            actionButton.Animate()
                .TranslationY(0)
                .SetInterpolator(new DecelerateInterpolator(2f))
                .Start();

            return actionButton;
        }

        public static FloatingActionButton HideWithTranslateAnimation(this FloatingActionButton actionButton)
        {
            var marginLayoutParams = actionButton.LayoutParameters as ViewGroup.MarginLayoutParams;
            int margin = marginLayoutParams?.BottomMargin ?? 0;

            actionButton.Animate()
                .TranslationY(actionButton.Height + margin)
                .SetInterpolator(new AccelerateInterpolator(2f))
                .Start();

            return actionButton;
        }
    }
}