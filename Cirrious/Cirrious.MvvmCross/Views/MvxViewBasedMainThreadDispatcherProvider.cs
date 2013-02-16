using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Views
{
    public class MvxViewBasedMainThreadDispatcherProvider
        : IMvxMainThreadDispatcherProvider
    {
        private IMvxViewDispatcherProvider _underlying;

        public MvxViewBasedMainThreadDispatcherProvider(IMvxViewDispatcherProvider underlying)
        {
            _underlying = underlying;
        }

        public IMvxMainThreadDispatcher Dispatcher { get { return _underlying.ViewDispatcher; } }
    }
}