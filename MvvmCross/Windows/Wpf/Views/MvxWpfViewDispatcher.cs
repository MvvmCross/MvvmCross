// MvxWpfViewDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Views
{
    using System.Windows.Threading;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public class MvxWpfViewDispatcher
        : MvxWpfUIThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxWpfViewPresenter _presenter;

        public MvxWpfViewDispatcher(Dispatcher dispatcher, IMvxWpfViewPresenter presenter)
            : base(dispatcher)
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