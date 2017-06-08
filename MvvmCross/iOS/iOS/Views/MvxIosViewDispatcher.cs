// MvxIosViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System;

    using Core.ViewModels;
    using Core.Views;
    using MvvmCross.Platform.Platform;
    using Presenters;

    public class MvxIosViewDispatcher
        : MvxIosUIThreadDispatcher
          , IMvxViewDispatcher
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
                    MvxTrace.TaggedTrace("iOSNavigation", "Navigate requested");
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