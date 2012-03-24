using Android.App;

namespace Cirrious.Conference.UI.Droid
{
    public static class ViewHelpers
    {
        public static void SetBackground(this Activity activity)
        {
            var drawable = activity.Resources.GetDrawable(Resource.Drawable.background);
            drawable.SetDither(true);
            activity.Window.SetBackgroundDrawable(drawable);
            activity.Window.DecorView.Background.SetDither(true);
        }
    }
}