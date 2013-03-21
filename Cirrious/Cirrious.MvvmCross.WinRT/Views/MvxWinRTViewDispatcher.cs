﻿// MvxWinRTViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxWinRTViewDispatcher
        : MvxWinRTMainThreadDispatcher
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

        public bool RequestNavigate(MvxViewModelRequest request)
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