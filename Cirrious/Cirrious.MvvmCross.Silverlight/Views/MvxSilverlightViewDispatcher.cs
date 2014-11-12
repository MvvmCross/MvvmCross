using System.Windows.Controls;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Silverlight.Views
{
    public class MvxSilverlightViewDispatcher
        : MvxSilverlightMainThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxSilverlightViewPresenter _presenter;
        private readonly Frame _rootFrame;

        public MvxSilverlightViewDispatcher(IMvxSilverlightViewPresenter presenter, Frame rootFrame)
            : base(rootFrame.Dispatcher)
        {
            _presenter = presenter;
            _rootFrame = rootFrame;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            return RequestMainThreadAction(() => _presenter.Show(request));
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            return RequestMainThreadAction(() => _presenter.ChangePresentation(hint));
        }
    }
}