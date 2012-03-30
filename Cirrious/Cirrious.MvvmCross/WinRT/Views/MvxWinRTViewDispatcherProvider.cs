#region Copyright
// <copyright file="MvxWinRTViewDispatcherProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxWinRTViewDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        private readonly IMvxWinRTViewPresenter _presenter;
        private readonly Frame _rootFrame;

        public MvxWinRTViewDispatcherProvider(IMvxWinRTViewPresenter presenter, Frame frame)
        {
            _presenter = presenter;
            _rootFrame = frame;
        }

        public IMvxViewDispatcher Dispatcher
        {
            get { return new MvxWinRTViewDispatcher(_presenter, _rootFrame); }
        }
    }
}