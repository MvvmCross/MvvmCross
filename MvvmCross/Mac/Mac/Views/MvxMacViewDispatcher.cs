namespace MvvmCross.Mac.Views
{
    using System;

    using global::MvvmCross.Core.ViewModels;
    using global::MvvmCross.Core.Views;
    using global::MvvmCross.Platform.Platform;

    using MvvmCross.Mac.Views.Presenters;

    public class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxMacViewPresenter _presenter;

        public MvxMacViewDispatcher(IMvxMacViewPresenter presenter)
        {
            this._presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
            {
                MvxTrace.TaggedTrace("MacNavigation", "Navigate requested");
                this._presenter.Show(request);
            };
            return this.RequestMainThreadAction(action);
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            Action action = () =>
                                {
                                    MvxTrace.TaggedTrace("MacNavigation", "Change presentation requested");
                                    this._presenter.ChangePresentation(hint);
                                };
            return this.RequestMainThreadAction(action);
        }
    }
}