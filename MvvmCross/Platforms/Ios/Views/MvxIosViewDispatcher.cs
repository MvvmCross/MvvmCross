// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Logging;
using MvvmCross.Platform.Ios.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platform.Ios.Views
{
    public class MvxIosViewDispatcher
        : MvxIosUIThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxIosViewPresenter _presenter;

        public MvxIosViewDispatcher(IMvxIosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
                {
                    MvxLog.Instance.Trace("iOSNavigation", "Navigate requested");
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
