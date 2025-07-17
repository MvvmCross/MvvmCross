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
    }
}
