using MvvmCross.WeakSubscription;
using Xunit;

namespace MvvmCross.UnitTest.Base
{
    public class MvxWeakEventSubscriptionTest
    {
        private sealed class DisposableClass : IDisposable
        {
            private bool _isDisposed;
            private EventHandler _testEvent;

            public event EventHandler TestEvent
            {
                add
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(DisposableClass));

                    _testEvent += value;
                }
                remove
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(DisposableClass));

                    _testEvent -= value;
                }
            }

            public void Dispose()
            {
                _isDisposed = true;
            }
        }

        [Fact]
        public void DisposeWeakEventSubscription_OnDisposedObject_DoesNotThrow()
        {
            var disposableClass = new DisposableClass();

            var subscription = new MvxWeakEventSubscription<DisposableClass>(disposableClass, "TestEvent", (_, _) =>
            {
            });

            disposableClass.Dispose();
            subscription.Dispose();

            Assert.True(true);
        }
    }
}
