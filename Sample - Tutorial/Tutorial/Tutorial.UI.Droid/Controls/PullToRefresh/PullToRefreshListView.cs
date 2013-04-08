using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Tutorial.UI.Droid.Controls.PullToRefresh
{
    public class PullToRefreshListView
        : RelativeLayout
        
    {
        private readonly ListView _listView;
        private readonly Handler _uiThreadHandler;
        private ViewGroup _lowerButton;
        private PullToRefreshComponent _pullToRefresh;
        private ViewGroup _upperButton;

        public PullToRefreshListView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public PullToRefreshListView(Context context, IAttributeSet attrs)
            : this(context, attrs, Resource.Layout.pull_to_refresh_list)
        {
        }

        public PullToRefreshListView(Context context, IAttributeSet attrs, int whichResourceId)
            : base(context, attrs)
        {
            var bindingContext = MvxAndroidBindingContextHelpers.Current();
            bindingContext.BindingInflate(whichResourceId, this);
            _listView = FindViewById<ListView>(global::Android.Resource.Id.List);
            _uiThreadHandler = new Handler();
            InitializePullToRefreshList();
        }


        public ListView UnderlyingListView
        {
            get { return _listView; }
        }

        private void InitializePullToRefreshList()
        {
            InitializeRefreshUpperButton();
            InitializeRefreshLowerButton();
            _pullToRefresh = new PullToRefreshComponent(
                _upperButton, _lowerButton, _listView,
                _uiThreadHandler);

            _pullToRefresh.SetOnReleaseUpperReady(new OnReleaseReadyUpper(this));
            _pullToRefresh.SetOnReleaseLowerReady(new OnReleaseReadyLower(this));
        }

        public void SetOnPullDownRefreshAction(IRefreshListener listener)
        {
            _pullToRefresh.SetOnPullDownRefreshAction(new PullDownRefreshListener(this, listener));
        }

        protected void RunOnUIThread(Action action)
        {
            _uiThreadHandler.Post(action);
        }

        public void SetOnPullUpRefreshAction(IRefreshListener listener)
        {
            _pullToRefresh.SetOnPullUpRefreshAction(new PullUpRefreshListener(this, listener));
        }

        private void InitializeRefreshLowerButton()
        {
            _upperButton = FindViewById<ViewGroup>(Resource.Id.refresh_upper_button);
        }

        private void InitializeRefreshUpperButton()
        {
            _lowerButton = FindViewById<ViewGroup>(Resource.Id.refresh_lower_button);
        }

        protected void SetPullToRefresh(ViewGroup refreshView)
        {
            string text = "";
            if (refreshView == _upperButton)
            {
                text = "PullDownToRefresh";
            }
            else
            {
                text = "PullUpToRefresh";
            }

            RefreshTextView(refreshView).Text = (text);
            RefreshProgressBar(refreshView).Visibility = ViewStates.Invisible;
            RefreshImage(refreshView).Visibility = ViewStates.Visible;
        }

        private static View RefreshImage(ViewGroup refreshView)
        {
            return refreshView.FindViewById(Resource.Id.pull_to_refresh_image);
        }

        private static View RefreshProgressBar(ViewGroup refreshView)
        {
            return refreshView.FindViewById(Resource.Id.pull_to_refresh_progress);
        }

        private static TextView RefreshTextView(ViewGroup refreshView)
        {
            return (refreshView.FindViewById<TextView>(Resource.Id.pull_to_refresh_text));
        }

        protected void SetRefreshing(ViewGroup refreshView)
        {
            RefreshTextView(refreshView).Text = "RefreshStarted";
            RefreshProgressBar(refreshView).Visibility = ViewStates.Visible;
            RefreshImage(refreshView).Visibility = ViewStates.Invisible;
        }

        public void DisablePullUpToRefresh()
        {
            _pullToRefresh.DisablePullUpToRefresh();
        }

        public void DisablePullDownToRefresh()
        {
            _pullToRefresh.DisablePullDownToRefresh();
        }

        #region Nested type: BaseNested

        private class BaseNested
        {
            protected readonly PullToRefreshListView Parent;

            public BaseNested(PullToRefreshListView parent)
            {
                Parent = parent;
            }
        }

        #endregion

        #region Nested type: OnReleaseReadyLower

        private class OnReleaseReadyLower : BaseNested, IOnReleaseReady
        {
            public OnReleaseReadyLower(PullToRefreshListView parent)
                : base(parent)
            {
            }

            #region IOnReleaseReady Members

            public void ReleaseReady(bool ready)
            {
                string textKey = ready
                                  ? "ReleaseToRefresh"
                                  : "PullUpToRefresh";
                RefreshTextView(Parent._lowerButton).Text = textKey;
            }

            #endregion
        }

        #endregion

        #region Nested type: OnReleaseReadyUpper

        private class OnReleaseReadyUpper : BaseNested, IOnReleaseReady
        {
            public OnReleaseReadyUpper(PullToRefreshListView parent)
                : base(parent)
            {
            }

            #region IOnReleaseReady Members

            public void ReleaseReady(bool ready)
            {
                string textKey = ready
                                  ? "ReleaseToRefresh"
                                  : "PullDownToRefresh";
                RefreshTextView(Parent._upperButton).Text = textKey;
            }

            #endregion
        }

        #endregion

        #region Nested type: PullDownRefreshListener

        private class PullDownRefreshListener : BaseNested, IRefreshListener
        {
            private readonly IRefreshListener _listener;

            public PullDownRefreshListener(PullToRefreshListView parent, IRefreshListener listener)
                : base(parent)
            {
                _listener = listener;
            }

            #region IRefreshListener Members

            public void DoRefresh()
            {
                Parent._uiThreadHandler.Post(() =>
                                                {
                                                    Parent.SetRefreshing(Parent._upperButton);
                                                    Parent.Invalidate();
                                                });
                _listener.DoRefresh();
            }

            public void RefreshFinished()
            {
                Parent.RunOnUIThread(() =>
                                         {
                                             Parent.SetPullToRefresh(Parent._upperButton);
                                             Parent.Invalidate();
                                             _listener.RefreshFinished();
                                         });
            }

            #endregion
        }

        #endregion

        #region Nested type: PullUpRefreshListener

        private class PullUpRefreshListener : BaseNested, IRefreshListener
        {
            private readonly IRefreshListener _listener;

            public PullUpRefreshListener(PullToRefreshListView parent, IRefreshListener listener)
                : base(parent)
            {
                _listener = listener;
            }

            #region IRefreshListener Members

            public void DoRefresh()
            {
                Parent._uiThreadHandler.Post(() =>
                                                {
                                                    Parent.SetRefreshing(Parent._lowerButton);
                                                    Parent.Invalidate();
                                                });
                _listener.DoRefresh();
            }

            public void RefreshFinished()
            {
                Parent.SetPullToRefresh(Parent._lowerButton);
                Parent.Invalidate();
                _listener.RefreshFinished();
            }

            #endregion
        }

        #endregion
    }
}