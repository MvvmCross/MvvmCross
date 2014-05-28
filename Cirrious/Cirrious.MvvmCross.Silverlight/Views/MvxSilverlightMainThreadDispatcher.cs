using System;
using System.Windows.Threading;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.Silverlight.Views
{
    public class MvxSilverlightMainThreadDispatcher : MvxMainThreadDispatcher
    {
       private readonly Dispatcher _uiDispatcher;

       public MvxSilverlightMainThreadDispatcher(Dispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

        private bool InvokeOrBeginInvoke(Action action)
        {
            if (_uiDispatcher.CheckAccess())
               ExceptionMaskedAction(action);
            else
                _uiDispatcher.BeginInvoke(action);

            return true;
        }
    }
}