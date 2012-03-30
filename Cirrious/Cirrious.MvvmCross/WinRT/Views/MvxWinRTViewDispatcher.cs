#region Copyright
// <copyright file="MvxWinRTViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Cirrious.MvvmCross.WinRT.Platform;
using Windows.UI.Xaml.Controls;

#endregion

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxWinRTViewDispatcher 
        : MvxMainThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxWinRTViewPresenter _presenter;
        private readonly Frame _rootFrame;

        public MvxWinRTViewDispatcher(IMvxWinRTViewPresenter presenter, Frame rootFrame)
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

        public bool RequestClose(IMvxViewModel viewModel)
        {
            return RequestMainThreadAction(() => _presenter.Close(viewModel));
        }

        public bool RequestRemoveBackStep()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}