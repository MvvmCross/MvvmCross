#region Copyright
// <copyright file="MvxWpfDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Windows.Threading;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Wpf.Interfaces;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public class MvxWpfDispatcher
        : MvxWpfUIThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxWpfViewPresenter _presenter;

        public MvxWpfDispatcher(Dispatcher dispatcher, IMvxWpfViewPresenter presenter)
            : base(dispatcher)
        {
            _presenter = presenter;
        }

        #region IMvxViewDispatcher Members

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            return InvokeOrBeginInvoke(() => _presenter.Show(request));
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            return InvokeOrBeginInvoke(() => _presenter.Close(toClose));
        }

        public bool RequestRemoveBackStep()
        {
            throw new NotImplementedException("This simple WPF framework does not implement RemoveBackStep");
        }

        #endregion

        private bool InvokeOrBeginInvoke(Action action)
        {
            action();
            return true;
        }
    }
}