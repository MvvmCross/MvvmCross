// MvxStoreMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsStore.Views
{
    using System;

    using Windows.UI.Core;

    using MvvmCross.Platform.Core;

    public class MvxStoreMainThreadDispatcher : MvxMainThreadDispatcher
    {
        private readonly CoreDispatcher _uiDispatcher;

        public MvxStoreMainThreadDispatcher(CoreDispatcher uiDispatcher)
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