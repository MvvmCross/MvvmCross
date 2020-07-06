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
        : MvxMainThreadDispatcher
        , IMvxViewDispatcher
    {
        public override bool IsOnMainThread => true;

        public override void ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            ExecuteOnMainThread(action);
        }

        public override ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            return ExceptionMaskedActionAsync(action, maskExceptions);
        }

        public ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            var navigation = Mvx.IoCProvider.Resolve<IMvxConsoleNavigation>();
            ExecuteOnMainThread(() => navigation.Show(request));

            return new ValueTask<bool>(true);
        }

        public ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            var navigation = Mvx.IoCProvider.Resolve<IMvxConsoleNavigation>();
            ExecuteOnMainThread(() => navigation.ChangePresentation(hint));

            return new ValueTask<bool>(true);
        }
    }
}
