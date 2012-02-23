using Android.Views;

namespace Tutorial.UI.Droid.Controls.PullToRefresh
{
    public class RefreshingState : IScrollingState
    {
        #region IScrollingState Members

        public bool TouchStopped(MotionEvent motionEvent,
                                 PullToRefreshComponent pullToRefreshComponent)
        {
            return true;
        }

        public bool HandleMovement(MotionEvent motionEvent,
                                   PullToRefreshComponent pullToRefreshComponent)
        {
            return true;
        }

        #endregion
    }
}