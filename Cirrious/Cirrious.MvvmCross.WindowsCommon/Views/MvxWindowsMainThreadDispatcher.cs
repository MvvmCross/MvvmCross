﻿// MvxStoreMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using System;
using Windows.UI.Core;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    public class MvxWindowsMainThreadDispatcher : MvxMainThreadDispatcher
    {
        private readonly CoreDispatcher _uiDispatcher;

        public MvxWindowsMainThreadDispatcher(CoreDispatcher uiDispatcher)
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

            _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ExceptionMaskedAction(action));
            return true;
        }
    }
}