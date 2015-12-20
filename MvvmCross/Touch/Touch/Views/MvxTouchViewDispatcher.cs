// MvxTouchViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views
{
    using System;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Touch.Views.Presenters;

    public class MvxTouchViewDispatcher
        : MvxTouchUIThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxTouchViewPresenter _presenter;

        public MvxTouchViewDispatcher(IMvxTouchViewPresenter presenter)
        {
            this._presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
                {
                    MvxTrace.TaggedTrace("TouchNavigation", "Navigate requested");
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