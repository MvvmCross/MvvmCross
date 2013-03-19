using System;
using Cirrious.CrossCore.Core;

namespace TwitterSearch.Test.Mocks
{
    public class MockMvxMainThreadDispatcher : IMvxMainThreadDispatcher
    {
        public bool RequestMainThreadAction(Action action)
        {
            action();
            return true;
        }
    }
}