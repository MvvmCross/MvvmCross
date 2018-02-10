// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platform.Console.Views
{
    public class MvxConsoleViewDispatcher
        : MvxMainThreadDispatcher
        , IMvxViewDispatcher
    {
        public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            action();
            return true;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return RequestMainThreadAction(() => navigation.Show(request));
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return RequestMainThreadAction(() => navigation.ChangePresentation(hint));
        }
    }
}
