// <copyright file="MvxTouchViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.Platform.Diagnostics;

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