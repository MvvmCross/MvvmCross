// MvxWinRTViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
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