using System;

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Platform;

using MvvmCross.Mac.Views.Presenters;

namespace MvvmCross.Mac.Views
{
    public class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxMacViewPresenter _presenter;

        public MvxMacViewDispatcher(IMvxMacViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
            {
                MvxTrace.TaggedTrace("MacNavigation", "Navigate requested");
                _presenter.Show(request);
            };
            return RequestMainThreadAction(action);
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            Action action = () =>
                                {
                                    MvxTrace.TaggedTrace("MacNavigation", "Change presentation requested");
                                    _presenter.ChangePresentation(hint);
                                };
            return RequestMainThreadAction(action);
        }
    }
}