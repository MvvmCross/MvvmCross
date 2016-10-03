// MvxTvosViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using System;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Platform;
    using MvvmCross.tvOS.Views.Presenters;

    public class MvxTvosViewDispatcher
        : MvxTvosUIThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxTvosViewPresenter _presenter;

        public MvxTvosViewDispatcher(IMvxTvosViewPresenter presenter)
        {
            this._presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
                {
                    MvxTrace.TaggedTrace("tvOSNavigation", "Navigate requested");
                    this._presenter.Show(request);
                };
            return this.RequestMainThreadAction(action);
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            return this.RequestMainThreadAction(() => this._presenter.ChangePresentation(hint));
        }
    }
}