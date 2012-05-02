#region Copyright
// <copyright file="MvxTouchViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
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
#warning What to do with ios back stack?
            // not supported on iOS really
            return false;
        }

        #endregion
    }
}