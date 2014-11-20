using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    public class MvxWindowsLoadStateEventArgs : EventArgs
    {
        public MvxWindowsLoadStateEventArgs(object navigationParameter, Dictionary<string, object> pageState)
        {
            NavigationParameter = navigationParameter;
            PageState = pageState;
        }

        /// <summary>A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited. </summary>
        public Dictionary<string, Object> PageState { get; private set; }

        /// <summary>The parameter value passed to <see cref="MvxWindowsFrame.Navigate"/> 
        /// when this page was initially requested. </summary>
        public Object NavigationParameter { get; private set; }
    }
}