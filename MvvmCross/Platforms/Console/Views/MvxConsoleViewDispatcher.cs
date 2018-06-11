// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Console.Views
{
    public class MvxConsoleViewDispatcher
        : MvxMainThreadAsyncDispatcher
        , IMvxViewDispatcher
    {
        public override bool IsOnMainThread => throw new NotImplementedException();

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            action();
            return true;
        }

        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            await ExecuteOnMainThreadAsync(() => navigation.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            await ExecuteOnMainThreadAsync(() => navigation.ChangePresentation(hint));
            return true;
        }
    }
}
