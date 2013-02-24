// MvxMacViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Mac.Views
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

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            Action action = () =>
                {
                    MvxTrace.TaggedTrace("MacNavigation", "Navigate requested");
                    _presenter.Show(request);
                };
            return RequestMainThreadAction(action);
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            Action action = () =>
                {
                    MvxTrace.TaggedTrace("MacNavigation", "Navigate back requested");
                    _presenter.Close(toClose);
                };
            return RequestMainThreadAction(action);
        }

        public bool RequestRemoveBackStep()
        {
#warning What to do with ios back stack?
            // not supported on Mac really
            return false;
        }

        #endregion
    }
}