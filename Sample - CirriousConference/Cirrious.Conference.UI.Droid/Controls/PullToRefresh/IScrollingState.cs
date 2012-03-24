using Android.Views;

namespace Cirrious.Conference.UI.Droid.Controls.PullToRefresh
{
    public interface IScrollingState
    {
        bool TouchStopped(MotionEvent motionEvent,
                          PullToRefreshComponent pullToRefreshComponent);

        bool HandleMovement(MotionEvent motionEvent,
                            PullToRefreshComponent pullToRefreshComponent);
    }
}