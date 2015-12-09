// MvxStoreViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsStore.Views
{
    using Windows.UI.Xaml.Controls;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public class MvxStoreViewDispatcher
        : MvxStoreMainThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxStoreViewPresenter _presenter;
        private readonly Frame _rootFrame;

        public MvxStoreViewDispatcher(IMvxStoreViewPresenter presenter, Frame rootFrame)
            : base(rootFrame.Dispatcher)
        {
            this._presenter = presenter;
            this._rootFrame = rootFrame;
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