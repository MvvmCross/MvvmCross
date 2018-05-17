﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Uap.Views
{
    public class MvxWindowsViewDispatcher
        : MvxWindowsMainThreadDispatcher, IMvxViewDispatcher
    {
        public IMvxViewPresenter Presenter { get; set; }

        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => Presenter.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => Presenter.ChangePresentation(hint));
            return true;
        }
    }
}
