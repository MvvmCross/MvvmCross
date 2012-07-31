#region Copyright
// <copyright file="MvxMainThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using Cirrious.MvvmCross.Interfaces.Views;
using Windows.UI.Core;

#endregion

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxMainThreadDispatcher : IMvxMainThreadDispatcher
    {
        private readonly CoreDispatcher _uiDispatcher;

        public MvxMainThreadDispatcher(CoreDispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        #region IMvxMainThreadDispatcher Members

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

        #endregion

        private bool InvokeOrBeginInvoke(Action action)
        {
            // TODO - could consider using _uiDispatcher.get_HasThreadAccess()
            _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            return true;
        }
    }
}