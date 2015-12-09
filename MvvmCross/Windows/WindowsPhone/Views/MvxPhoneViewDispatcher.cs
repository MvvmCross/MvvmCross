// MvxPhoneViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Views
{
    using Microsoft.Phone.Controls;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public class MvxPhoneViewDispatcher
        : MvxPhoneMainThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxPhoneViewPresenter _presenter;
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewDispatcher(IMvxPhoneViewPresenter presenter, PhoneApplicationFrame rootFrame)
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