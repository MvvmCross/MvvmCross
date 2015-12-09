// MvxStoreViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsCommon.Views
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public class MvxWindowsViewDispatcher
        : MvxWindowsMainThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxWindowsViewPresenter _presenter;

        public MvxWindowsViewDispatcher(IMvxWindowsViewPresenter presenter, IMvxWindowsFrame rootFrame)
            : base(rootFrame.UnderlyingControl.Dispatcher)
        {
            this._presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            return this.RequestMainThreadAction(() => this._presenter.Show(request));
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            return this.RequestMainThreadAction(() => this._presenter.ChangePresentation(hint));
        }
    }
}