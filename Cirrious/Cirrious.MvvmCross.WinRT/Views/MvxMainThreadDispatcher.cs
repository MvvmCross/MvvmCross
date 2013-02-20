// MvxMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


using System;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Interfaces.Views;
using Windows.UI.Core;

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
            var method = _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            return true;
        }
    }
}