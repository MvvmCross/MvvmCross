// MvxStoreViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Uwp.Views
{
    public class MvxWindowsViewDispatcher
        : MvxWindowsMainThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxWindowsViewPresenter _presenter;

        public MvxWindowsViewDispatcher(IMvxWindowsViewPresenter presenter, IMvxWindowsFrame rootFrame)
            : base(rootFrame.UnderlyingControl.Dispatcher)
        {
            _presenter = presenter;
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