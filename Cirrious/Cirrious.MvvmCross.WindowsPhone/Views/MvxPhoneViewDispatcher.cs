// MvxPhoneViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Microsoft.Phone.Controls;

#endregion

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneViewDispatcher
        : MvxPhoneMainThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxPhoneViewPresenter _presenter;
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewDispatcher(IMvxPhoneViewPresenter presenter, PhoneApplicationFrame rootFrame)
            : base(rootFrame.Dispatcher)
        {
            _presenter = presenter;
            _rootFrame = rootFrame;
        }

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            return RequestMainThreadAction(() => _presenter.Show(request));
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            return RequestMainThreadAction(() => _presenter.Close(toClose));
        }

        public bool RequestRemoveBackStep()
        {
            return RequestMainThreadAction(() => _rootFrame.RemoveBackEntry());
        }

        #endregion
    }
}