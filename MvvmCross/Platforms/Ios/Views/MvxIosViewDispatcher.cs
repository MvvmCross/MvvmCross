// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Ios.Views
{
    public class MvxIosViewDispatcher
        : MvxIosUIThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxIosViewPresenter _presenter;

        public MvxIosViewDispatcher(IMvxIosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(async () =>
                {
                    MvxLog.Instance.Trace("iOSNavigation", "Navigate requested");
                    await _presenter.Show(request).ConfigureAwait(false);
                }).ConfigureAwait(false);

            return true;
        }

        public async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(async () =>
            {
                await _presenter.ChangePresentation(hint).ConfigureAwait(false);
            }).ConfigureAwait(false);

            return true;
        }
    }
}
