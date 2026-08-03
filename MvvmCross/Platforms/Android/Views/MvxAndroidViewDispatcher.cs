// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxAndroidViewDispatcher
        : MvxAndroidMainThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxAndroidViewPresenter _presenter;

        public MvxAndroidViewDispatcher(IMvxAndroidViewPresenter presenter)
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
