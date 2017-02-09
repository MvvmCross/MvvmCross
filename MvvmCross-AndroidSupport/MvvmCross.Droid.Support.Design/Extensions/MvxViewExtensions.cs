using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.Animations;

namespace MvvmCross.Droid.Support.Design.Extensions
{
    public static class MvxViewExtensions
    {
        public static View ShowWithTranslateAnimation(this View view)
        {
            view.Animate()
                .TranslationY(0)
                .SetInterpolator(new DecelerateInterpolator(2f))
                .Start();

            return view;
        }

        public static View HideWithTranslateAnimation(this View view)
        {
            var marginLayoutParams = view.LayoutParameters as ViewGroup.MarginLayoutParams;
            int margin = marginLayoutParams?.BottomMargin ?? 0;

            view.Animate()
                .TranslationY(view.Height + margin)
                .SetInterpolator(new AccelerateInterpolator(2f))
                .Start();

            return view;
        }
    }
}