using Cirrious.CrossCore.Core;

namespace TwitterSearch.Test.Mocks
{
    public class MockMvxMainThreadDispatcherProvider : IMvxMainThreadDispatcherProvider
    {
        public IMvxMainThreadDispatcher Dispatcher { get { return new MockMvxMainThreadDispatcher(); } }
    }
}