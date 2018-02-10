// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Core;
using MvvmCross.Base;

namespace MvvmCross.Platform.Uap.Views
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
