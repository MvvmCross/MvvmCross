using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    public class MvxWindowsSaveStateEventArgs : EventArgs
    {
        public MvxWindowsSaveStateEventArgs(Dictionary<string, Object> pageState)
        {
            PageState = pageState;
        }

        /// <summary>A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited. </summary>
        public Dictionary<string, Object> PageState { get; private set; }
    }
}