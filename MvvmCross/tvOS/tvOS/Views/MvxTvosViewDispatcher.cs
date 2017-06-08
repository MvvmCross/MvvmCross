// MvxTvosViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using System;

    using Core.ViewModels;
    using Core.Views;
    using MvvmCross.Platform.Platform;
    using Presenters;

    public class MvxTvosViewDispatcher
        : MvxTvosUIThreadDispatcher
          , IMvxViewDispatcher
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
                    MvxTrace.TaggedTrace("tvOSNavigation", "Navigate requested");
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