// MvxViewBasedMainThreadDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Views
{
    public class MvxViewBasedMainThreadDispatcherProvider
        : IMvxMainThreadDispatcherProvider
    {
        private readonly IMvxViewDispatcherProvider _underlying;

        public MvxViewBasedMainThreadDispatcherProvider(IMvxViewDispatcherProvider underlying)
        {
            _underlying = underlying;
        }

        public IMvxMainThreadDispatcher Dispatcher
        {
            get { return _underlying.ViewDispatcher; }
        }
    }
}