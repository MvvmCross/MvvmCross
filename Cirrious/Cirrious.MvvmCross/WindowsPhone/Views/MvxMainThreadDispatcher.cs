#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MvxCrossThreadDispatcher.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Interfaces.Views;

#endregion

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxMainThreadDispatcher : IMvxMainThreadDispatcher
    {
        private readonly Dispatcher _uiDispatcher;
        private bool _stopRequested = false;

        public MvxMainThreadDispatcher(Dispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

#warning RequestStop removed at present
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