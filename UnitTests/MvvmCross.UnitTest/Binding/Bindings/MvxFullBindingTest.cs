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
using System.Collections.Concurrent;

namespace MvvmCross.UnitTest.Binding.Bindings
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxFullBindingTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxFullBindingTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Test]
        public async Task TestTwoWayEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.TwoWay,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            await TwoWayAssertions(binding, mockTarget, mockSource);
        }

        [Test]
        public async Task TestDefaultTwoWayEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.TwoWay,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            await TwoWayAssertions(binding, mockTarget, mockSource);
        }

        private static async Task TwoWayAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            await Assert.That(mockTarget.SubscribeToEventsCalled).IsEqualTo(1);

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("TryGetValueValue");

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsEqualTo("SecondValue");

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsEqualTo("ThirdValue");

            await Assert.That(mockSource.ValuesSet).IsEmpty();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            await Assert.That(mockSource.ValuesSet).HasSingleItem();
            await Assert.That(mockSource.ValuesSet[0]).IsEqualTo("FromTarget1");
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            await Assert.That(mockSource.ValuesSet.Count).IsEqualTo(2);
            await Assert.That(mockSource.ValuesSet[1]).IsEqualTo("FromTarget2");

            await Assert.That(mockSource.DisposeCalled).IsEqualTo(0);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            binding.DataContext = new { ignored = 12 };
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(1);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values.Count).IsEqualTo(4);
            await Assert.That(mockTarget.Values[3]).IsEqualTo("ThirdValue");

            binding.DataContext = new { ignored = 13 };
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(2);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values.Count).IsEqualTo(5);
            await Assert.That(mockTarget.Values[4]).IsEqualTo("ThirdValue");

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values.Count).IsEqualTo(6);
            await Assert.That(mockTarget.Values[5]).IsEqualTo("NewValue");

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            await Assert.That(mockSource.ValuesSet.Count).IsEqualTo(3);
            await Assert.That(mockSource.ValuesSet[2]).IsEqualTo("FromTarget1");
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            await Assert.That(mockSource.ValuesSet.Count).IsEqualTo(4);
            await Assert.That(mockSource.ValuesSet[3]).IsEqualTo("FromTarget2");

            binding.Dispose();
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(3);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(1);
        }

        [Test]
        public async Task TestOneWayEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.OneWay, out mockSource, out mockTarget);

            await OneWayAssertions(binding, mockTarget, mockSource);
        }

        [Test]
        public async Task TestDefaultOneWayEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneWay,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            await OneWayAssertions(binding, mockTarget, mockSource);
        }

        private static async Task OneWayAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            await Assert.That(mockTarget.SubscribeToEventsCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("TryGetValueValue");

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsEqualTo("SecondValue");

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsEqualTo("ThirdValue");

            await Assert.That(mockSource.ValuesSet).IsEmpty();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            await Assert.That(mockSource.ValuesSet).IsEmpty();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            await Assert.That(mockSource.ValuesSet).IsEmpty();

            await Assert.That(mockSource.DisposeCalled).IsEqualTo(0);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            binding.DataContext = new { ignored = 12 };
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(1);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values.Count).IsEqualTo(4);
            await Assert.That(mockTarget.Values[3]).IsEqualTo("ThirdValue");

            binding.DataContext = new { ignored = 13 };
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(2);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values.Count).IsEqualTo(5);
            await Assert.That(mockTarget.Values[4]).IsEqualTo("ThirdValue");

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values.Count).IsEqualTo(6);
            await Assert.That(mockTarget.Values[5]).IsEqualTo("NewValue");

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            await Assert.That(mockSource.ValuesSet).IsEmpty();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            await Assert.That(mockSource.ValuesSet).IsEmpty();

            binding.Dispose();
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(3);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(1);
        }

        [Test]
        public async Task TestOneWayToSourceEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.OneWayToSource,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            await OnWayToSourceAssertions(binding, mockTarget, mockSource);
        }

        [Test]
        public async Task TestDefaultOneWayToSourceEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneWayToSource,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            await OnWayToSourceAssertions(binding, mockTarget, mockSource);
        }

        private static async Task OnWayToSourceAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            await Assert.That(mockTarget.SubscribeToEventsCalled).IsEqualTo(1);

            await Assert.That(mockTarget.Values).IsEmpty();

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values).IsEmpty();

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values).IsEmpty();

            await Assert.That(mockSource.ValuesSet).IsEmpty();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            await Assert.That(mockSource.ValuesSet).HasSingleItem();
            await Assert.That(mockSource.ValuesSet[0]).IsEqualTo("FromTarget1");
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            await Assert.That(mockSource.ValuesSet.Count).IsEqualTo(2);
            await Assert.That(mockSource.ValuesSet[1]).IsEqualTo("FromTarget2");

            await Assert.That(mockSource.DisposeCalled).IsEqualTo(0);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            binding.DataContext = new { ignored = 12 };
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(1);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values).IsEmpty();

            binding.DataContext = new { ignored = 13 };
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(2);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values).IsEmpty();

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values).IsEmpty();

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            await Assert.That(mockSource.ValuesSet.Count).IsEqualTo(3);
            await Assert.That(mockSource.ValuesSet[2]).IsEqualTo("FromTarget1");
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            await Assert.That(mockSource.ValuesSet.Count).IsEqualTo(4);
            await Assert.That(mockSource.ValuesSet[3]).IsEqualTo("FromTarget2");

            binding.Dispose();
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(3);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(1);
        }

        [Test]
        public async Task TestOneTimeEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.OneTime,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            await OneTimeAssertions(binding, mockTarget, mockSource);
        }

        [Test]
        public async Task TestDefaultOneTimeEventSubscription()
        {
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneTime,
                out MockSourceBinding mockSource, out MockTargetBinding mockTarget);

            await OneTimeAssertions(binding, mockTarget, mockSource);
        }

        private static async Task OneTimeAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            await Assert.That(mockTarget.SubscribeToEventsCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("TryGetValueValue");

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values).HasSingleItem();

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values).HasSingleItem();

            await Assert.That(mockSource.ValuesSet).IsEmpty();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            await Assert.That(mockSource.ValuesSet).IsEmpty();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            await Assert.That(mockSource.ValuesSet).IsEmpty();

            await Assert.That(mockSource.DisposeCalled).IsEqualTo(0);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            binding.DataContext = new { ignored = 12 };
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(1);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsEqualTo("ThirdValue");

            binding.DataContext = new { ignored = 13 };
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(2);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(0);

            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsEqualTo("ThirdValue");

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            await Assert.That(mockSource.ValuesSet).IsEmpty();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            await Assert.That(mockSource.ValuesSet).IsEmpty();

            binding.Dispose();
            await Assert.That(mockSource.DisposeCalled).IsEqualTo(3);
            await Assert.That(mockTarget.DisposeCalled).IsEqualTo(1);
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

        [Test]
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
                            await Task.Delay(1, TestContext.Current.Execution.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.Execution.CancellationToken));
            }

            await Task.WhenAll(tasks);

            // Assert
            await Assert.That(exceptions).IsEmpty();
        }

        [Test]
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
                            await Task.Delay(1, TestContext.Current.Execution.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.Execution.CancellationToken),
                Task.Run(async () =>
                {
                    try
                    {
                        // Let some source changes happen first
                        await Task.Delay(50, TestContext.Current.Execution.CancellationToken);
                        for (int i = 0; i < 10; i++)
                        {
                            binding.DataContext = new { Value = i };
                            await Task.Delay(10, TestContext.Current.Execution.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.Execution.CancellationToken)
            ];

            await Task.WhenAll(tasks);

            // Assert
            await Assert.That(exceptions).IsEmpty();
        }

        [Test]
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
                            await Task.Delay(1, TestContext.Current.Execution.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.Execution.CancellationToken),

                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 500; i++)
                        {
                            binding.DataContext = new { Value = i };
                            await Task.Delay(1, TestContext.Current.Execution.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.Execution.CancellationToken)
            ];

            await Task.WhenAll(tasks);

            // Assert
            await Assert.That(exceptions).IsEmpty();
        }

        [Test]
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
                        await Task.Delay(1, TestContext.Current.Execution.CancellationToken);
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
            }, TestContext.Current.Execution.CancellationToken);

            await Task.Delay(50, TestContext.Current.Execution.CancellationToken);
            binding.Dispose();
            keepRunning = false;

            await readTask;

            // Assert - No null reference exceptions should occur
            await Assert.That(exceptions).IsEmpty();
        }

        [Test]
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
                }, TestContext.Current.Execution.CancellationToken));
            }

            await Task.WhenAll(tasks);
            binding.Dispose();

            // Assert
            await Assert.That(exceptions).IsEmpty();
        }

        [Test]
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
                }, TestContext.Current.Execution.CancellationToken));
            }

            await Task.WhenAll(tasks);

            // Assert - Dispose should be safe to call multiple times
            await Assert.That(exceptions).IsEmpty();
        }

        [Test]
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
                            await Task.Delay(1, TestContext.Current.Execution.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.Execution.CancellationToken),

                // Target changes
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 500 && keepRunning; i++)
                        {
                            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs($"Target{i}"));
                            await Task.Delay(1, TestContext.Current.Execution.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.Execution.CancellationToken),

                // DataContext changes
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 100 && keepRunning; i++)
                        {
                            binding.DataContext = new { Value = i };
                            await Task.Delay(5, TestContext.Current.Execution.CancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }, TestContext.Current.Execution.CancellationToken),

                // Read ActualBindingMode
                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 1000 && keepRunning; i++)
                        {
                            _ = binding.ActualBindingMode;
                            await Task.Delay(1, TestContext.Current.Execution.CancellationToken);
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
                }, TestContext.Current.Execution.CancellationToken)
            ];

            await Task.WhenAll(tasks);
            keepRunning = false;
            binding.Dispose();

            // Assert
            await Assert.That(exceptions).IsEmpty();
        }
    }
}
