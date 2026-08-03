// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Uap.Views
{
    public class MvxWindowsViewDispatcher
        : MvxWindowsMainThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxWindowsViewPresenter _presenter;

        public MvxWindowsViewDispatcher(IMvxWindowsViewPresenter presenter, IMvxWindowsFrame rootFrame)
            : base(rootFrame.UnderlyingControl.Dispatcher)
        {
            _presenter = presenter;
        }

        [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
