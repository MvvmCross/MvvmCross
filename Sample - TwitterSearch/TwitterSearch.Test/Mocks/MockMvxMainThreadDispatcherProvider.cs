using Cirrious.CrossCore.Interfaces.Core;

namespace TwitterSearch.Test.Mocks
{
    public class MockMvxMainThreadDispatcherProvider : IMvxMainThreadDispatcherProvider
    {
        public IMvxMainThreadDispatcher Dispatcher { get { return new MockMvxMainThreadDispatcher(); } }
    }
}