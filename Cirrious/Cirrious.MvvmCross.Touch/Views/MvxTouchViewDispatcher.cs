// MvxTouchViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTouchViewDispatcher
        : MvxTouchUIThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxTouchViewPresenter _presenter;

        public MvxTouchViewDispatcher(IMvxTouchViewPresenter presenter)
        {
            _presenter = presenter;
        }

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            Action action = () =>
                {
                    MvxTrace.TaggedTrace("TouchNavigation", "Navigate requested");
                    _presenter.Show(request);
                };
            return RequestMainThreadAction(action);
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            Action action = () =>
                {
                    MvxTrace.TaggedTrace("TouchNavigation", "Navigate back requested");
                    _presenter.Close(toClose);
                };
            return RequestMainThreadAction(action);
        }

        public bool RequestRemoveBackStep()
        {
            return RequestMainThreadAction(() =>
                {
                    MvxTrace.TaggedTrace("TouchNavigation", "Request back step removed");
                    _presenter.RequestRemoveBackStep();
                });
        }

        #endregion
    }
}