// MvxStoreMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsCommon.Views
{
    using System;

    using Windows.UI.Core;

    using MvvmCross.Platform.Core;

    public class MvxWindowsMainThreadDispatcher : MvxMainThreadDispatcher
    {
        private readonly CoreDispatcher _uiDispatcher;

        public MvxWindowsMainThreadDispatcher(CoreDispatcher uiDispatcher)
        {
            this._uiDispatcher = uiDispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            if (this._uiDispatcher.HasThreadAccess)
            {
                action();
                return true;
            }

            this._uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ExceptionMaskedAction(action));
            return true;
        }
    }
}