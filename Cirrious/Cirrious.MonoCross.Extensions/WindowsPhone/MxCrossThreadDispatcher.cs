using System;
using System.Windows.Threading;
using Cirrious.MonoCross.Extensions.Interfaces;

namespace Cirrious.MonoCross.Extensions.WindowsPhone
{
    public class MxCrossThreadDispatcher : IMXCrossThreadDispatcher, IMXStopNowPlease
    {
        private readonly Dispatcher _uiDispatcher;
        private bool _stopRequested = false;

        public MxCrossThreadDispatcher(Dispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

        public void RequestStop()
        {
            _stopRequested = true;
        }

        protected bool InvokeOrBeginInvoke(Action action)
        {
            if (_stopRequested)
                return false;

            if (_uiDispatcher.CheckAccess())
                action();
            else
                _uiDispatcher.BeginInvoke(() =>
                                              {
                                                  if (_stopRequested)
                                                      return;
                                                  action();
                                              });

            return true;
        }
    }
}