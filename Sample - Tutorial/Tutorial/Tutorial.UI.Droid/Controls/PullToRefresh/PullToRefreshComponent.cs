using System;
using System.Threading;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Math = System.Math;

namespace Tutorial.UI.Droid.Controls.PullToRefresh
{
    public class PullToRefreshComponent
    {
        public const float MinPullElementHeight = 100;

        private const float MaxPullElementHeight = 200;
        private const float PullElementStandbyHeight = 100;
        private const int EventCount = 3;

        private readonly float[] _lastYs = new float[EventCount];
        private readonly ListView _listView;
        private readonly View _lowerView;
        private readonly Handler _uiThreadHandler;
        private readonly View _upperView;
        private bool _mayPullDownToRefresh;
        private bool _mayPullUpToRefresh = true;

        private IRefreshListener _onPullDownRefreshAction = new NoRefreshListener();
        private IRefreshListener _onPullUpRefreshAction = new NoRefreshListener();

        private IOnReleaseReady _onReleaseLowerReady;
        private IOnReleaseReady _onReleaseUpperReady;
        protected IScrollingState State;

        public PullToRefreshComponent(View upperView, View lowerView,
                                      ListView listView, Handler uiThreadHandler)
        {
            this._upperView = upperView;
            this._lowerView = lowerView;
            this._listView = listView;
            this._uiThreadHandler = uiThreadHandler;
            Initialize();
        }

        private void Initialize()
        {
            State = new PullToRefreshState();
            _listView.Touch += (sender, args) =>
                                  {
                                      MotionEvent motionEvent = args.Event;
                                      if (motionEvent.Action == MotionEventActions.Up)
                                      {
                                          InitializeYsHistory();
                                          State.TouchStopped(motionEvent, this);
                                      }
                                      else if (motionEvent.Action == MotionEventActions.Move)
                                      {
                                          args.Handled = State.HandleMovement(motionEvent, this);
                                      }
                                      args.Handled = false;
                                  };
        }

        protected float Average(float[] ysArray)
        {
            float avg = 0;
            for (int i = 0; i < EventCount; i++)
            {
                avg += ysArray[i];
            }
            return avg/EventCount;
        }

        public void BeginPullDownRefresh()
        {
            BeginRefresh(_upperView, _onPullDownRefreshAction);
        }

        private void BeginRefresh(View viewToUpdate,
                                  IRefreshListener refreshAction)
        {
            ViewGroup.LayoutParams layoutParams = viewToUpdate.LayoutParameters;
            layoutParams.Height = (int) PullElementStandbyHeight;
            viewToUpdate.LayoutParameters = layoutParams;
            //UITrace.Trace("PullDown:refreshing");
            State = new RefreshingState();
            ThreadPool.QueueUserWorkItem((ignored) =>
                                             {
                                                 try
                                                 {
                                                     //var start = DateTime.UtcNow;
                                                     refreshAction.DoRefresh();
                                                     //var finish = DateTime.UtcNow;
                                                     //long difference = finish - start;
                                                     //try
                                                     //{
                                                     //    Thread.Sleep(Math.Max(difference, 1500));
                                                     //}
                                                     //catch (InterruptedException e)
                                                     //{
                                                     //}
                                                 }
                                                 catch (RuntimeException e)
                                                 {
                                                     //UITrace.Trace("Error: {0}", e.ToLongString());
                                                     throw e;
                                                 }
                                                 finally
                                                 {
                                                     RunOnUiThread(() => RefreshFinished(refreshAction));
                                                 }
                                             });
        }


        public void BeginPullUpRefresh()
        {
            BeginRefresh(_lowerView, _onPullUpRefreshAction);
        }

        /**************************************************************/
        // Listeners
        /**************************************************************/

        public void SetOnPullDownRefreshAction(IRefreshListener onRefreshAction)
        {
            EnablePullDownToRefresh();
            _onPullDownRefreshAction = onRefreshAction;
        }

        public void SetOnPullUpRefreshAction(IRefreshListener onRefreshAction)
        {
            EnablePullUpToRefresh();
            _onPullUpRefreshAction = onRefreshAction;
        }

        public void RefreshFinished(IRefreshListener refreshAction)
        {
            //UITrace.Trace("PullDown: ready");
            State = new PullToRefreshState();
            InitializeYsHistory();
            RunOnUiThread(() =>
                              {
                                  float dp = new Pixel(0, _listView.Resources).ToDp();
                                  SetUpperButtonHeight(dp);
                                  SetLowerButtonHeight(dp);
                                  refreshAction.RefreshFinished();
                              });
        }

        private void RunOnUiThread(Action action)
        {
            _uiThreadHandler.Post(action);
        }

        private void SetUpperButtonHeight(float height)
        {
            SetHeight(height, _upperView);
        }

        private void SetLowerButtonHeight(float height)
        {
            SetHeight(height, _lowerView);
        }

        private void SetHeight(float height, View view)
        {
            if (view == null)
            {
                return;
            }
            ViewGroup.LayoutParams layoutParams = view.LayoutParameters;
            if (layoutParams == null)
            {
                return;
            }
            layoutParams.Height = (int) height;
            view.LayoutParameters = layoutParams;
            view.Parent.RequestLayout();
        }

        public int GetListTop()
        {
            return _listView.Top;
        }

        public void InitializeYsHistory()
        {
            for (int i = 0; i < EventCount; i++)
            {
                _lastYs[i] = 0;
            }
        }

        /**************************************************************/
        // HANDLE PULLING
        /**************************************************************/

        public void PullDown(MotionEvent mevent, float firstY)
        {
            float averageY = Average(_lastYs);

            var height = (int) Math.Max(
                Math.Min(averageY - firstY, MaxPullElementHeight), 0);
            SetUpperButtonHeight(height);
        }

        public void PullUp(MotionEvent mevent, float firstY)
        {
            float averageY = Average(_lastYs);

            var height = (int) Math.Max(Math.Min(firstY - averageY, MaxPullElementHeight), 0);
            SetLowerButtonHeight(height);
        }

        public bool IsPullingDownToRefresh()
        {
            return _mayPullDownToRefresh && IsIncremental()
                   && IsFirstVisible();
        }

        public bool IsPullingUpToRefresh()
        {
            return _mayPullUpToRefresh && IsDecremental()
                   && IsLastVisible();
        }

        private bool IsFirstVisible()
        {
            if (_listView.Count == 0)
            {
                return true;
            }
            else if (_listView.FirstVisiblePosition == 0)
            {
                return _listView.GetChildAt(0).Top >= _listView.Top;
            }
            else
            {
                return false;
            }
        }

        private bool IsLastVisible()
        {
            if (_listView.Count == 0)
            {
                return true;
            }
            else if (_listView.LastVisiblePosition + 1 == _listView.Count)
            {
                return _listView.GetChildAt(_listView.ChildCount - 1)
                           .Bottom <= _listView.Bottom;
            }
            else
            {
                return false;
            }
        }

        private bool IsIncremental(int from, int to, int step)
        {
            float realFrom = _lastYs[from];
            float realTo = _lastYs[to];
            //UITrace.Trace("pull to refresh scrolling from " + realFrom);
            //UITrace.Trace("pull to refresh scrolling to " + realTo);
            return _lastYs[from] != 0 && realTo != 0
                   && Math.Abs(realFrom - realTo) > 50 && realFrom*step < realTo;
        }

        private bool IsIncremental()
        {
            return IsIncremental(0, EventCount - 1, +1);
        }

        private bool IsDecremental()
        {
            return IsIncremental(0, EventCount - 1, -1);
        }

        public void UpdateEventStates(MotionEvent motionEvent)
        {
            for (int i = 0; i < EventCount - 1; i++)
            {
                _lastYs[i] = _lastYs[i + 1];
            }

            float y = motionEvent.GetY();
            int top = _listView.Top;
            //UITrace.Trace("Pulltorefresh event y:" + y);
            //UITrace.Trace("Pulltorefresh list top:" + top);
            _lastYs[EventCount - 1] = y + top;
        }

        /**************************************************************/
        // State Change
        /**************************************************************/

        public void SetPullingDown(MotionEvent motionEvent)
        {
            //UITrace.Trace("PullDown pulling down");
            State = new PullingDownState(motionEvent);
        }

        public void SetPullingUp(MotionEvent motionEvent)
        {
            //UITrace.Trace("PullDown pulling up");
            State = new PullingUpState(motionEvent);
        }

        public float GetBottomViewHeight()
        {
            return _lowerView.Height;
        }

        public IRefreshListener GetOnUpperRefreshAction()
        {
            return _onPullDownRefreshAction;
        }

        public IRefreshListener GetOnLowerRefreshAction()
        {
            return _onPullUpRefreshAction;
        }

        public void DisablePullUpToRefresh()
        {
            _mayPullUpToRefresh = false;
        }

        public void DisablePullDownToRefresh()
        {
            _mayPullDownToRefresh = false;
        }

        public void EnablePullUpToRefresh()
        {
            _mayPullUpToRefresh = true;
        }

        public void EnablePullDownToRefresh()
        {
            _mayPullDownToRefresh = true;
        }

        public void SetOnReleaseUpperReady(IOnReleaseReady onReleaseUpperReady)
        {
            this._onReleaseUpperReady = onReleaseUpperReady;
        }

        public void SetOnReleaseLowerReady(IOnReleaseReady onReleaseUpperReady)
        {
            _onReleaseLowerReady = onReleaseUpperReady;
        }

        public void ReadyToReleaseUpper(bool ready)
        {
            if (_onReleaseUpperReady != null)
            {
                _onReleaseUpperReady.ReleaseReady(ready);
            }
        }

        public void ReadyToReleaseLower(bool ready)
        {
            if (_onReleaseLowerReady != null)
            {
                _onReleaseLowerReady.ReleaseReady(ready);
            }
        }
    }
}