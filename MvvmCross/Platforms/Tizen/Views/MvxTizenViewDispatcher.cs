// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Platforms.Tizen.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Tizen.Views
{
    public class MvxTizenViewDispatcher
        : MvxTizenMainThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxTizenViewPresenter _presenter;

        public MvxTizenViewDispatcher(IMvxTizenViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
            {
                MvxLog.Instance.Trace("TizenNavigation", "Navigate requested");
                _presenter.Show(request);
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
