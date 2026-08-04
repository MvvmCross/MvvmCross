// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using Xunit;

namespace MvvmCross.UnitTest.Base
{
    public class MvxMainThreadAsyncDispatcherTest
    {
        private const int TestTimeoutMs = 10_000;

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_ActionCompletes_ReturnedTaskCompletes()
        {
            var dispatcher = new InlineMockMainThreadDispatcher();
            var executed = false;

            await dispatcher.ExecuteOnMainThreadAsync(() => { executed = true; });

            Assert.True(executed);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_ActionThrows_WithMaskExceptionsFalse_ReturnedTaskFaults()
        {
            var dispatcher = new InlineMockMainThreadDispatcher();
            Action action = () => throw new InvalidOperationException("boom");

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions: false));

            Assert.Equal("boom", exception.Message);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_ActionThrows_WithMaskExceptionsTrue_ReturnedTaskCompletes()
        {
            var dispatcher = new InlineMockMainThreadDispatcher();
            Action action = () => throw new InvalidOperationException("boom");

            await dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions: true);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_AsyncActionThrowsAfterAwait_WithMaskExceptionsFalse_ReturnedTaskFaults()
        {
            var dispatcher = new InlineMockMainThreadDispatcher();
            Func<Task> action = async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException("boom");
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions: false));

            Assert.Equal("boom", exception.Message);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_AsyncActionThrowsAfterAwait_WithMaskExceptionsTrue_ReturnedTaskCompletes()
        {
            var dispatcher = new InlineMockMainThreadDispatcher();
            Func<Task> action = async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException("boom");
            };

            await dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions: true);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_ActionCompletes_WhenDispatchedFromBackgroundThread_ReturnedTaskCompletes()
        {
            var dispatcher = new BackgroundMockMainThreadDispatcher();
            var executed = false;

            await dispatcher.ExecuteOnMainThreadAsync(() => { executed = true; });

            Assert.True(executed);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_ActionThrows_WhenDispatchedFromBackgroundThread_ReturnedTaskFaults()
        {
            var dispatcher = new BackgroundMockMainThreadDispatcher();
            Action action = () => throw new InvalidOperationException("boom");

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions: false));

            Assert.Equal("boom", exception.Message);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_ActionThrows_WhenDispatchedFromBackgroundThread_WithMaskExceptionsTrue_ReturnedTaskCompletes()
        {
            var dispatcher = new BackgroundMockMainThreadDispatcher();
            Action action = () => throw new InvalidOperationException("boom");

            await dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions: true);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_AsyncActionThrows_WhenDispatchedFromBackgroundThread_ReturnedTaskFaults()
        {
            var dispatcher = new BackgroundMockMainThreadDispatcher();
            Func<Task> action = async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException("boom");
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions: false));

            Assert.Equal("boom", exception.Message);
        }

        [Fact(Timeout = TestTimeoutMs)]
        public async Task ExecuteOnMainThreadAsync_AsyncActionThrows_WhenDispatchedFromBackgroundThread_WithMaskExceptionsTrue_ReturnedTaskCompletes()
        {
            var dispatcher = new BackgroundMockMainThreadDispatcher();
            Func<Task> action = async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException("boom");
            };

            await dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions: true);
        }
    }
}
