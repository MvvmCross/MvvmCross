// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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

        public async ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(async() =>
                {
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
