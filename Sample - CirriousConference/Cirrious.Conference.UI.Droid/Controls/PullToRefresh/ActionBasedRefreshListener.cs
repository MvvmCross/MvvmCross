using System;

namespace Cirrious.Conference.UI.Droid.Controls.PullToRefresh
{
    public class ActionBasedRefreshListener : IRefreshListener
    {
        private readonly Action _action;

        public ActionBasedRefreshListener(Action action)
        {
            _action = action;
        }

        #region IRefreshListener Members

        public void DoRefresh()
        {
            _action();
        }

        public void RefreshFinished()
        {
        }

        #endregion
    }
}