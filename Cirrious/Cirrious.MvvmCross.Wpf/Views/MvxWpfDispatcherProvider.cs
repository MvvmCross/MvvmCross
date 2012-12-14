#region Copyright
// <copyright file="MvxWpfDispatcherProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Windows.Threading;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Wpf.Interfaces;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public class MvxWpfDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        private readonly Dispatcher _uiThreadDispatcher;
        private readonly IMvxWpfViewPresenter _presenter;

        public  MvxWpfDispatcherProvider(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
        {
            _presenter = presenter;
            _uiThreadDispatcher = uiThreadDispatcher;
        }

        public IMvxViewDispatcher Dispatcher
        {
            get { return new MvxWpfDispatcher(_uiThreadDispatcher, _presenter); }
        }
    }
}