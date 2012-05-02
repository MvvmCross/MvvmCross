using System;
using System.Threading;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Util;
using Tutorial.UI.Droid.Controls.PullToRefresh;

namespace Tutorial.UI.Droid.Controls
{
    public class BindingPullToRefreshListView
        : PullToRefreshListView
    {
        public BindingPullToRefreshListView(Context context, IAttributeSet attrs)
            : base(context, attrs, Resource.Layout.pull_to_refresh_bindable_list)
        {
            SetOnPullUpRefreshAction(new BindingListener(OnPullUpRefreshRequested, WaitForTail));
            SetOnPullDownRefreshAction(new BindingListener(OnPullDownRefreshRequested, WaitForHead));
        }

        private class BindingListener : IRefreshListener
        {
            readonly Action _requestAction;
            readonly Action _waitAction;

            public BindingListener(Action requestAction, Action waitAction)
            {
                _requestAction = requestAction;
                _waitAction = waitAction;
            }

            public void DoRefresh()
            {
                _requestAction();                
                _waitAction();
            }

            public void RefreshFinished()
            {
                // ignored
            }
        }

        public ICommand PullDownRefreshRequested { get; set; }

        public void OnPullDownRefreshRequested()
        {
            var handler = PullDownRefreshRequested;
            SyncFireHandler(handler);
        }

        private readonly ManualResetEvent _manualResetEventHead = new ManualResetEvent(true);
        public bool IsRefreshingHead
        {
            get { return _manualResetEventHead.WaitOne(0); }
            set
            {
                if (value)
                    _manualResetEventHead.Reset();
                else
                    _manualResetEventHead.Set();
            }
        }

        private void WaitForHead()
        {
            _manualResetEventHead.WaitOne();
        }

        public ICommand PullUpRefreshRequested { get; set; }

        public void OnPullUpRefreshRequested()
        {
            var handler = PullUpRefreshRequested;
            SyncFireHandler(handler);
        }

        private readonly ManualResetEvent _manualResetEventTail = new ManualResetEvent(true);
        public bool IsRefreshingTail
        {
            get { return _manualResetEventTail.WaitOne(0); }
            set
            {
                if (value)
                    _manualResetEventTail.Reset();
                else
                    _manualResetEventTail.Set();
            }
        }

        private void WaitForTail()
        {
            _manualResetEventTail.WaitOne();
        }

        private void SyncFireHandler(ICommand handler)
        {
            if (handler != null)
            {
                var waitForHandler = new ManualResetEvent(false);
                ((Activity)Context).RunOnUiThread(
                    () =>
                    {
                        handler.Execute(null);
                        waitForHandler.Set();
                    });
                waitForHandler.WaitOne();
            }
        }
    }
}