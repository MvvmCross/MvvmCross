// MvxMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Windows.UI.Core;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxWinRTMainThreadDispatcher : MvxMainThreadDispatcher
    {
        private readonly CoreDispatcher _uiDispatcher;

        public MvxWinRTMainThreadDispatcher(CoreDispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            if (_uiDispatcher.HasThreadAccess)
            {
                action();
                return true;
            }

            _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            return true;
        }
    }
}