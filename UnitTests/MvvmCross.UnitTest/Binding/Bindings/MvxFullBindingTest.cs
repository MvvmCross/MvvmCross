// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.UnitTest.Binding.Mocks;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using NSubstitute;
using Xunit;
using System.Collections.Concurrent;

namespace MvvmCross.UnitTest.Binding.Bindings
{
    [Collection("MvxTest")]
    public class MvxFullBindingTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxFullBindingTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestTwoWayEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.TwoWay,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            TwoWayAssertions(binding, mockTarget, mockSource);
        }

        [Fact]
        public void TestDefaultTwoWayEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.TwoWay,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            TwoWayAssertions(binding, mockTarget, mockSource);
        }

        private static void TwoWayAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            Assert.Equal(1, mockTarget.SubscribeToEventsCalled);

            Assert.Single(mockTarget.Values);
            Assert.Equal("TryGetValueValue", mockTarget.Values[0]);

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Equal("SecondValue", mockTarget.Values[1]);

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Equal("ThirdValue", mockTarget.Values[2]);

            Assert.Empty(mockSource.ValuesSet);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.Single(mockSource.ValuesSet);
            Assert.Equal("FromTarget1", mockSource.ValuesSet[0]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.Equal(2, mockSource.ValuesSet.Count);
            Assert.Equal("FromTarget2", mockSource.ValuesSet[1]);

            Assert.Equal(0, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.Equal(1, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            Assert.Equal(4, mockTarget.Values.Count);
            Assert.Equal("ThirdValue", mockTarget.Values[3]);

            binding.DataContext = new { ignored = 13 };
            Assert.Equal(2, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            Assert.Equal(5, mockTarget.Values.Count);
            Assert.Equal("ThirdValue", mockTarget.Values[4]);

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            Assert.Equal(6, mockTarget.Values.Count);
            Assert.Equal("NewValue", mockTarget.Values[5]);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.Equal(3, mockSource.ValuesSet.Count);
            Assert.Equal("FromTarget1", mockSource.ValuesSet[2]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.Equal(4, mockSource.ValuesSet.Count);
            Assert.Equal("FromTarget2", mockSource.ValuesSet[3]);

            binding.Dispose();
            Assert.Equal(3, mockSource.DisposeCalled);
            Assert.Equal(1, mockTarget.DisposeCalled);
        }

        [Fact]
        public void TestOneWayEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.OneWay, out mockSource, out mockTarget);

            OneWayAssertions(binding, mockTarget, mockSource);
        }

        [Fact]
        public void TestDefaultOneWayEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneWay,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            OneWayAssertions(binding, mockTarget, mockSource);
        }

        private static void OneWayAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            Assert.Equal(0, mockTarget.SubscribeToEventsCalled);

            Assert.Single(mockTarget.Values);
            Assert.Equal("TryGetValueValue", mockTarget.Values[0]);

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Equal("SecondValue", mockTarget.Values[1]);

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Equal("ThirdValue", mockTarget.Values[2]);

            Assert.Empty(mockSource.ValuesSet);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.Empty(mockSource.ValuesSet);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.Empty(mockSource.ValuesSet);

            Assert.Equal(0, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.Equal(1, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            Assert.Equal(4, mockTarget.Values.Count);
            Assert.Equal("ThirdValue", mockTarget.Values[3]);

            binding.DataContext = new { ignored = 13 };
            Assert.Equal(2, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            Assert.Equal(5, mockTarget.Values.Count);
            Assert.Equal("ThirdValue", mockTarget.Values[4]);

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            Assert.Equal(6, mockTarget.Values.Count);
            Assert.Equal("NewValue", mockTarget.Values[5]);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.Empty(mockSource.ValuesSet);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.Empty(mockSource.ValuesSet);

            binding.Dispose();
            Assert.Equal(3, mockSource.DisposeCalled);
            Assert.Equal(1, mockTarget.DisposeCalled);
        }

        [Fact]
        public void TestOneWayToSourceEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.OneWayToSource,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            OnWayToSourceAssertions(binding, mockTarget, mockSource);
        }

        [Fact]
        public void TestDefaultOneWayToSourceEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneWayToSource,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            OnWayToSourceAssertions(binding, mockTarget, mockSource);
        }

        private static void OnWayToSourceAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            Assert.Equal(1, mockTarget.SubscribeToEventsCalled);

            Assert.Empty(mockTarget.Values);

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            Assert.Empty(mockTarget.Values);

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            Assert.Empty(mockTarget.Values);

            Assert.Empty(mockSource.ValuesSet);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.Single(mockSource.ValuesSet);
            Assert.Equal("FromTarget1", mockSource.ValuesSet[0]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.Equal(2, mockSource.ValuesSet.Count);
            Assert.Equal("FromTarget2", mockSource.ValuesSet[1]);

            Assert.Equal(0, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.Equal(1, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            Assert.Empty(mockTarget.Values);

            binding.DataContext = new { ignored = 13 };
            Assert.Equal(2, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            Assert.Empty(mockTarget.Values);

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            Assert.Empty(mockTarget.Values);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.Equal(3, mockSource.ValuesSet.Count);
            Assert.Equal("FromTarget1", mockSource.ValuesSet[2]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.Equal(4, mockSource.ValuesSet.Count);
            Assert.Equal("FromTarget2", mockSource.ValuesSet[3]);

            binding.Dispose();
            Assert.Equal(3, mockSource.DisposeCalled);
            Assert.Equal(1, mockTarget.DisposeCalled);
        }

        [Fact]
        public void TestOneTimeEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.OneTime,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            OneTimeAssertions(binding, mockTarget, mockSource);
        }

        [Fact]
        public void TestDefaultOneTimeEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneTime,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            OneTimeAssertions(binding, mockTarget, mockSource);
        }

        private static void OneTimeAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            Assert.Equal(0, mockTarget.SubscribeToEventsCalled);

            Assert.Single(mockTarget.Values);
            Assert.Equal("TryGetValueValue", mockTarget.Values[0]);

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            Assert.Single(mockTarget.Values);

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            Assert.Single(mockTarget.Values);

            Assert.Empty(mockSource.ValuesSet);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.Empty(mockSource.ValuesSet);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.Empty(mockSource.ValuesSet);

            Assert.Equal(0, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.Equal(1, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Equal("ThirdValue", mockTarget.Values[1]);

            binding.DataContext = new { ignored = 13 };
            Assert.Equal(2, mockSource.DisposeCalled);
            Assert.Equal(0, mockTarget.DisposeCalled);

            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Equal("ThirdValue", mockTarget.Values[2]);

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            Assert.Equal(3, mockTarget.Values.Count);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.Empty(mockSource.ValuesSet);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.Empty(mockSource.ValuesSet);

            binding.Dispose();
            Assert.Equal(3, mockSource.DisposeCalled);
            Assert.Equal(1, mockTarget.DisposeCalled);
        }

        private MvxFullBinding TestSetupCommon(MvxBindingMode mvxBindingMode,
            out MockSourceBinding mockSource, out MockTargetBinding mockTarget)
        {
            return TestSetupCommon(mvxBindingMode, MvxBindingMode.Default, out mockSource, out mockTarget);
        }

        private MvxFullBinding TestSetupCommon(MvxBindingMode mvxBindingMode, MvxBindingMode defaultMode,
            out MockSourceBinding mockSource, out MockTargetBinding mockTarget)
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadAsyncDispatcher>(new InlineMockMainThreadDispatcher());

            var mockSourceBindingFactory = Substitute.For<IMvxSourceBindingFactory>();
            _fixture.Ioc.RegisterSingleton(mockSourceBindingFactory);

            var mockTargetBindingFactory = Substitute.For<IMvxTargetBindingFactory>();
            _fixture.Ioc.RegisterSingleton(mockTargetBindingFactory);

            var realSourceStepFactory = new MvxSourceStepFactory();
            realSourceStepFactory.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            _fixture.Ioc.RegisterSingleton<IMvxSourceStepFactory>(realSourceStepFactory);

            var sourceText = "sourceText";
            var targetName = "targetName";
            var source = new { Value = 1 };
            var target = new { Value = 2 };
            var converterParameter = new { Value = 3 };
            var fallbackValue = new { Value = 4 };
            var bindingDescription = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription
                {
                    Converter = null,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    SourcePropertyPath = sourceText,
                },
                Mode = mvxBindingMode,
                TargetName = targetName
            };

            mockSource = new MockSourceBinding();
            mockTarget = new MockTargetBinding { DefaultMode = defaultMode };

            var localSource = mockSource;
            mockSourceBindingFactory
                .CreateBinding(Arg.Any<object>(), Arg.Is<string>(s => s == sourceText))
                .Returns(localSource);

            var localTarget = mockTarget;
            mockTargetBindingFactory
                .CreateBinding(Arg.Any<object>(), Arg.Is<string>(s => s == targetName))
                .Returns(localTarget);

            mockSource.TryGetValueResult = true;
            mockSource.TryGetValueValue = "TryGetValueValue";

            var request = new MvxBindingRequest(source, target, bindingDescription);
            var toTest = new MvxFullBinding(request);
            return toTest;
        }

        [Fact]
        public async Task TestConcurrentDataContextChanges_ShouldNotThrow()
        {
            // Arrange
            using var binding = TestSetupCommon(MvxBindingMode.TwoWay,
                out MockSourceBinding _, out MockTargetBinding _);

            var exceptions = new ConcurrentBag<Exception>();
            var tasks = new List<Task>();
            var iterations = 100;

            // Act - Multiple threads changing DataContext simultaneously
            for (int i = 0; i < 10; i++)
            {
                int threadId = i;
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        for (int j = 0; j < iterations; j++)
                        {
                            binding.DataContext = new { Value = threadId * 1000 + j };
                            await Task.Delay(1, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken));
            }

            await Task.WhenAll(tasks);

            // Assert
            Assert.Empty(exceptions);
        }

        [Fact]
        public async Task TestConcurrentSourceChangesAndDispose_ShouldNotThrow()
        {
            // Arrange
            using var binding = TestSetupCommon(MvxBindingMode.OneWay,
                out MockSourceBinding mockSource, out MockTargetBinding _);

            var exceptions = new ConcurrentBag<Exception>();

            // Act - One thread firing source changes, another disposing

            Task[] tasks = [
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            mockSource.TryGetValueValue = $"Value{i}";
                            mockSource.FireSourceChanged();
                            await Task.Delay(1, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken),
                Task.Run(async () =>
                {
                    try
                    {
                        // Let some source changes happen first
                        await Task.Delay(50, TestContext.Current.CancellationToken);
                        for (int i = 0; i < 10; i++)
                        {
                            binding.DataContext = new { Value = i };
                            await Task.Delay(10, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken)
            ];

            await Task.WhenAll(tasks);

            // Assert
            Assert.Empty(exceptions);
        }

        [Fact]
        public async Task TestConcurrentTargetChangesAndDataContextChanges_ShouldNotThrow()
        {
            // Arrange
            using var binding = TestSetupCommon(MvxBindingMode.TwoWay,
                out MockSourceBinding _, out MockTargetBinding mockTarget);

            var exceptions = new ConcurrentBag<Exception>();

            // Act - Concurrent target value changes and data context changes
            Task[] tasks =
            [
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 500; i++)
                        {
                            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs($"TargetValue{i}"));
                            await Task.Delay(1, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken),

                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 500; i++)
                        {
                            binding.DataContext = new { Value = i };
                            await Task.Delay(1, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken)
            ];

            await Task.WhenAll(tasks);

            // Assert
            Assert.Empty(exceptions);
        }

        [Fact]
        public async Task TestActualBindingModeAccess_WhileDisposing_ShouldNotThrow()
        {
            // This tests the specific bug reported - accessing ActualBindingMode during disposal
            var binding = TestSetupCommon(MvxBindingMode.TwoWay,
                out MockSourceBinding _, out MockTargetBinding _);

            var exceptions = new ConcurrentBag<Exception>();
            var keepRunning = true;

            // Act - Read ActualBindingMode repeatedly while disposing
            var readTask = Task.Run(async () =>
            {
                try
                {
                    while (keepRunning)
                    {
                        _ = binding.ActualBindingMode;
                        await Task.Delay(1, TestContext.Current.CancellationToken);
                    }
                }
                catch (ObjectDisposedException)
                {
                    // This is acceptable - we're accessing after disposal
                }
                catch (Exception ex)
                {
                    // But null reference exceptions are NOT acceptable
                    exceptions.Add(ex);
                }
            }, TestContext.Current.CancellationToken);

            await Task.Delay(50, TestContext.Current.CancellationToken);
            binding.Dispose();
            keepRunning = false;

            await readTask;

            // Assert - No null reference exceptions should occur
            Assert.Empty(exceptions);
        }

        [Fact]
        public async Task TestConcurrentSourceChanges_WithCancellation_ShouldNotThrow()
        {
            // Tests that CancellationTokenSource operations are thread-safe
            var binding = TestSetupCommon(MvxBindingMode.OneWay,
                out MockSourceBinding mockSource, out MockTargetBinding _);

            var exceptions = new ConcurrentBag<Exception>();
            var tasks = new List<Task>();

            // Act - Multiple threads triggering source changes (which creates new CancellationTokenSources)
            for (int i = 0; i < 5; i++)
            {
                int threadId = i;
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            mockSource.TryGetValueValue = $"Thread{threadId}_Value{j}";
                            mockSource.FireSourceChanged();

                            // Also change DataContext which recreates CancellationTokenSource
                            if (j % 10 == 0)
                            {
                                binding.DataContext = new { ThreadId = threadId, Iteration = j };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken));
            }

            await Task.WhenAll(tasks);
            binding.Dispose();

            // Assert
            Assert.Empty(exceptions);
        }

        [Fact]
        public async Task TestConcurrentDispose_ShouldBeIdempotent()
        {
            // Arrange
            var binding = TestSetupCommon(MvxBindingMode.TwoWay,
                out MockSourceBinding _, out MockTargetBinding _);

            var exceptions = new ConcurrentBag<Exception>();
            var tasks = new List<Task>();

            // Act - Multiple threads trying to dispose simultaneously
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        binding.Dispose();
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken));
            }

            await Task.WhenAll(tasks);

            // Assert - Dispose should be safe to call multiple times
            Assert.Empty(exceptions);
        }

        [Fact]
        public async Task TestStressTest_AllOperationsConcurrently_ShouldNotThrow()
        {
            // Comprehensive stress test combining all operations
            var binding = TestSetupCommon(MvxBindingMode.TwoWay,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            var exceptions = new ConcurrentBag<Exception>();
            var keepRunning = true;

            Task[] tasks =
            [
                // Source changes
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 500 && keepRunning; i++)
                        {
                            mockSource.TryGetValueValue = $"Source{i}";
                            mockSource.FireSourceChanged();
                            await Task.Delay(1, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken),

                // Target changes
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 500 && keepRunning; i++)
                        {
                            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs($"Target{i}"));
                            await Task.Delay(1, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken),

                // DataContext changes
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 100 && keepRunning; i++)
                        {
                            binding.DataContext = new { Value = i };
                            await Task.Delay(5, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken),

                // Read ActualBindingMode
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 1000 && keepRunning; i++)
                        {
                            _ = binding.ActualBindingMode;
                            await Task.Delay(1, TestContext.Current.CancellationToken);
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // intentionally empty
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.CancellationToken)
            ];

            await Task.WhenAll(tasks);
            keepRunning = false;
            binding.Dispose();

            // Assert
            Assert.Empty(exceptions);
        }
    }
}
