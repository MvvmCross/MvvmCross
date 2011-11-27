#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXSimpleDispatcher.cs" company="Cirrious">
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
using System.Collections.Generic;
using System.ComponentModel;
using Cirrious.MonoCross.Extensions.Interfaces;
using MonoCross.Navigation;

#endregion

namespace Cirrious.MonoCross.Extensions.Android
{
    public class MXSimpleDispatcher : IMXViewDispatcher, IMXStopNowPlease
    {
        private bool _stopRequested = false;

        public void RequestStop()
        {
            _stopRequested = true;
        }

        private bool ExecuteNow(Action action)
        {
            if (_stopRequested)
                return false;

            action();
            return true;
        }

        public void NotifyPropertyChanged(string nameOfProperty)
        {
            ExecuteNow(() =>
                           {
                               if (PropertyChanged != null)
                                   PropertyChanged(this, new PropertyChangedEventArgs(nameOfProperty));
                           });
        }

        public bool RequestNavigate(string url, Dictionary<string, string> parameters)
        {
            return ExecuteNow(() =>
                                  {
                                      // note that we are passing a null "fromView" into this method
                                      // this is a shame - as it looks ugly - but seemed unavoidable...
                                      // is this fromView parameter really needed?
                                      MXContainer.Navigate(null, url, parameters);
                                  });
        }

        public bool RequestMainThreadAction(Action action)
        {
            return ExecuteNow(action);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}