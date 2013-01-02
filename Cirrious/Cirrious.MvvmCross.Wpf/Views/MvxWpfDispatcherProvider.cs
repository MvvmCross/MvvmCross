// MvxWpfDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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

        public MvxWpfDispatcherProvider(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
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