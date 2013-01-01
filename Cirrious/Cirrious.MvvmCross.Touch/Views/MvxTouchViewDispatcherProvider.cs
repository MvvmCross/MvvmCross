// MvxTouchViewDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTouchViewDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        private readonly IMvxTouchViewPresenter _presenter;

        public MvxTouchViewDispatcherProvider(IMvxTouchViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public IMvxViewDispatcher Dispatcher
        {
            get { return new MvxTouchViewDispatcher(_presenter); }
        }
    }
}