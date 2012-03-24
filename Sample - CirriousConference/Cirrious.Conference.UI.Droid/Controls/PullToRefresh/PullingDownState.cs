using Android.Views;

namespace Cirrious.Conference.UI.Droid.Controls.PullToRefresh
{
    public class PullingDownState : IScrollingState
    {
        private readonly float _firstY;

        public PullingDownState(MotionEvent motionEvent)
        {
            _firstY = motionEvent.GetY();
        }

        #region IScrollingState Members

        public bool TouchStopped(MotionEvent motionEvent,
                                 PullToRefreshComponent pullToRefreshComponent)
        {
            if (pullToRefreshComponent.GetListTop() > PullToRefreshComponent.MinPullElementHeight)
            {
                pullToRefreshComponent.BeginPullDownRefresh();
            }
            else
            {
                pullToRefreshComponent.RefreshFinished(pullToRefreshComponent.GetOnUpperRefreshAction());
            }
            return true;
        }

        public bool HandleMovement(MotionEvent motionEvent,
                                   PullToRefreshComponent pullToRefreshComponent)
        {
            pullToRefreshComponent.UpdateEventStates(motionEvent);
            pullToRefreshComponent.PullDown(motionEvent, _firstY);
            pullToRefreshComponent.ReadyToReleaseUpper(pullToRefreshComponent
                                                           .GetListTop() >
                                                       PullToRefreshComponent.MinPullElementHeight);
            return true;
        }

        #endregion
    }
}