// MvxStoreMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.UI.Core;
using MvvmCross.Platform.Core;

namespace MvvmCross.Uwp.Views
{
    public class MvxWindowsMainThreadDispatcher : MvxMainThreadDispatcher
    {
        private readonly CoreDispatcher _uiDispatcher;

        public MvxWindowsMainThreadDispatcher(CoreDispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (_uiDispatcher.HasThreadAccess)
            {
                action();
                return true;
            }

            _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
            {
                if (maskExceptions)
                    ExceptionMaskedAction(action);
                else
                    action();
            });
            return true;
        }
    }
}