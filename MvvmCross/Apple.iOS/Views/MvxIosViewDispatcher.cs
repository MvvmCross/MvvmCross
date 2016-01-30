// MvxIosViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Platform;
    using MvvmCross.iOS.Views.Presenters;

    public class MvxIosViewDispatcher
        : MvxIosUIThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxIosViewPresenter _presenter;

        public MvxIosViewDispatcher(IMvxIosViewPresenter presenter)
        {
            this._presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
                {
                    MvxTrace.TaggedTrace("iOSNavigation", "Navigate requested");
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