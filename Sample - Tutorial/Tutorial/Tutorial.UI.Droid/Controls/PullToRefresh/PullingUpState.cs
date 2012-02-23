using Android.Views;

namespace Tutorial.UI.Droid.Controls.PullToRefresh
{
    public class PullingUpState : IScrollingState
    {
        private readonly float _firstY;

        public PullingUpState(MotionEvent motionEvent)
        {
            _firstY = motionEvent.GetY();
        }

        #region IScrollingState Members

        public bool TouchStopped(MotionEvent motionEvent,
                                 PullToRefreshComponent pullToRefreshComponent)
        {
            if (pullToRefreshComponent.GetBottomViewHeight() > PullToRefreshComponent.MinPullElementHeight)
            {
                pullToRefreshComponent.BeginPullUpRefresh();
            }
            else
            {
                pullToRefreshComponent.RefreshFinished(pullToRefreshComponent
                                                           .GetOnLowerRefreshAction());
            }
            return true;
        }

        public bool HandleMovement(MotionEvent motionEvent,
                                   PullToRefreshComponent pullToRefreshComponent)
        {
            pullToRefreshComponent.UpdateEventStates(motionEvent);
            pullToRefreshComponent.PullUp(motionEvent, _firstY);
            pullToRefreshComponent
                .ReadyToReleaseLower(pullToRefreshComponent
                                         .GetBottomViewHeight() > PullToRefreshComponent.MinPullElementHeight);
            return true;
        }

        #endregion
    }
}