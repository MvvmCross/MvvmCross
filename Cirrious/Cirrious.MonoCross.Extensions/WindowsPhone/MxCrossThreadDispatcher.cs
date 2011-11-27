#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MxCrossThreadDispatcher.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

#region using

using System;
using System.Windows.Threading;
using Cirrious.MonoCross.Extensions.Interfaces;

#endregion

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