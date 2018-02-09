// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Logging;
using MvvmCross.Platform.Tvos.Views.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platform.Tvos.Views
{
    public class MvxTvosViewDispatcher
        : MvxTvosUIThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxTvosViewPresenter _presenter;

        public MvxTvosViewDispatcher(IMvxTvosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
                {
                    MvxLog.Instance.Trace("tvOSNavigation", "Navigate requested");
                    _presenter.Show(request);
                };
            return RequestMainThreadAction(action);
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            return RequestMainThreadAction(() => _presenter.ChangePresentation(hint));
        }
    }
}
