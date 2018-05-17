﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Platforms.Mac.Presenters;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Mac.Views
{
    public class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , IMvxViewDispatcher
    {
        public IMvxViewPresenter Presenter { get; set; }

        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
            {
                MvxLog.Instance.Trace("MacNavigation", "Navigate requested");
                Presenter.Show(request);
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            Action action = () =>
                                {
                                    MvxLog.Instance.Trace("MacNavigation", "Change presentation requested");
                                    Presenter.ChangePresentation(hint);
                                };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }
    }
}
