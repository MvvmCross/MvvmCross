// MvxMacViewDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Mac.Interfaces;

namespace Cirrious.MvvmCross.Mac.Views
{
    public class MvxMacViewDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        private readonly IMvxMacViewPresenter _presenter;

        public MvxMacViewDispatcherProvider(IMvxMacViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public IMvxViewDispatcher Dispatcher
        {
            get { return new MvxMacViewDispatcher(_presenter); }
        }
    }
}