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
using MvvmCross.Converters;
using MvvmCross.UnitTest.Binding.Mocks;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using NSubstitute;
using Xunit;

namespace MvvmCross.UnitTest.Binding.Bindings
{
    [Collection("MvxTest")]
    public class MvxFullBindingValueConversionTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxFullBindingValueConversionTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestConverterIsUsedForConvert()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "Test ConversionResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockSource, out mockTarget);

            Assert.Single(mockTarget.Values);
            Assert.Equal("Test ConversionResult", mockTarget.Values[0]);
            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Equal("TryGetValueValue", mockValueConverter.ConversionsRequested[0]);
        }

        [Fact]
        public void TestConverterParameterIsUsedForConvert()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "Test ConversionResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockSource, out mockTarget);

            Assert.Single(mockTarget.Values);
            Assert.Equal("Test ConversionResult", mockTarget.Values[0]);
            Assert.Single(mockValueConverter.ConversionParameters);
            Assert.Equal(parameter, mockValueConverter.ConversionParameters[0]);
        }

        [Fact]
        public void TestConverterIsUsedForConvertBack()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionBackResult = "Test ConversionBackResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockSource, out mockTarget);

            var valueChanged = new { Hello = 34 };
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs(valueChanged));

            Assert.Single(mockSource.ValuesSet);
            Assert.Equal("Test ConversionBackResult", mockSource.ValuesSet[0]);
            Assert.Single(mockValueConverter.ConversionsBackRequested);
            Assert.Equal(valueChanged, mockValueConverter.ConversionsBackRequested[0]);
        }

        [Fact]
        public void TestConverterParameterIsUsedForConvertBack()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionBackResult = "Test ConversionBackResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockSource, out mockTarget);

            var valueChanged = new { Hello = 34 };
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs(valueChanged));

            Assert.Single(mockSource.ValuesSet);
            Assert.Equal("Test ConversionBackResult", mockSource.ValuesSet[0]);
            Assert.Single(mockValueConverter.ConversionBackParameters);
            Assert.Equal(parameter, mockValueConverter.ConversionBackParameters[0]);
        }

        [Fact]
        public void TestTargetTypeIsUsedForConvert()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
            };
            var parameter = new { Ignored = 12 };
            var targetType = new { Foo = 23 }.GetType();
            var binding = TestSetupCommon(mockValueConverter, parameter, targetType, out mockSource, out mockTarget);

            mockSource.TryGetValueValue = "new value";
            mockSource.FireSourceChanged();

            Assert.Equal(2, mockValueConverter.ConversionTypes.Count);
            Assert.Equal(targetType, mockValueConverter.ConversionTypes[0]);
            Assert.Equal(targetType, mockValueConverter.ConversionTypes[1]);
        }

        [Fact]
        public void TestSourceTypeIsUsedForConvertBack()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockSource, out mockTarget);

            var aType = new { Hello = 34 };
            mockSource.SourceType = aType.GetType();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("Ignored"));

            Assert.Single(mockValueConverter.ConversionBackTypes);
            Assert.Equal(aType.GetType(), mockValueConverter.ConversionBackTypes[0]);
        }

        [Fact]
        public void TestFallbackValueIsUsedIfConversionFails()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            var fallback = new { Fred = "Not Barney" };
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(object), out mockSource, out mockTarget);

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Single(mockTarget.Values);
            Assert.Equal(fallback, mockTarget.Values[0]);
        }

        [Fact]
        public void TestFallbackValueIsUsedIfSourceResolutionFails()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            var fallback = new { Fred = "Not Barney" };
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(object), out mockSource, out mockTarget);

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Single(mockTarget.Values);
            Assert.Equal("A test value", mockTarget.Values[0]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Equal(fallback, mockTarget.Values[1]);
        }

        [Fact]
        public void TestDefaultValueIsUsedIfConversionFails_Object()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(object), out mockSource, out mockTarget);

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Single(mockTarget.Values);
            Assert.Null(mockTarget.Values[0]);

            mockSource.TryGetValueValue = "Fred";
            mockSource.FireSourceChanged();

            Assert.Equal(2, mockValueConverter.ConversionsRequested.Count);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[1]);

            mockSource.TryGetValueValue = "Betty";
            mockSource.FireSourceChanged();

            Assert.Equal(3, mockValueConverter.ConversionsRequested.Count);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[2]);
        }

        [Fact]
        public void TestDefaultValueIsUsedIfConversionFails_Int()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(int), out mockSource, out mockTarget);

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Single(mockTarget.Values);
            Assert.Equal(0, mockTarget.Values[0]);

            mockSource.TryGetValueValue = "Fred";
            mockSource.FireSourceChanged();

            Assert.Equal(2, mockValueConverter.ConversionsRequested.Count);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Equal(0, mockTarget.Values[1]);

            mockSource.TryGetValueValue = "Betty";
            mockSource.FireSourceChanged();

            Assert.Equal(3, mockValueConverter.ConversionsRequested.Count);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Equal(0, mockTarget.Values[2]);
        }

        [Fact]
        public void TestDefaultValueIsUsedIfConversionFails_NullableInt()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(int?), out mockSource, out mockTarget);

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Single(mockTarget.Values);
            Assert.Null(mockTarget.Values[0]);

            mockSource.TryGetValueValue = "Fred";
            mockSource.FireSourceChanged();

            Assert.Equal(2, mockValueConverter.ConversionsRequested.Count);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[1]);

            mockSource.TryGetValueValue = "Betty";
            mockSource.FireSourceChanged();

            Assert.Equal(3, mockValueConverter.ConversionsRequested.Count);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[2]);
        }

        [Fact]
        public void TestDefaultValueIsUsedIfSourceResolutionFails_Object()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(object), out mockSource, out mockTarget);

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Single(mockTarget.Values);
            Assert.Equal("A test value", mockTarget.Values[0]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[1]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[2]);

            mockSource.TryGetValueValue = "Fred";
            mockSource.TryGetValueResult = true;
            mockSource.FireSourceChanged();

            Assert.Equal(2, mockValueConverter.ConversionsRequested.Count);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(4, mockTarget.Values.Count);
            Assert.Equal("A test value", mockTarget.Values[3]);
        }

        [Fact]
        public void TestDefaultValueIsUsedIfSourceResolutionFails_Int()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(int), out mockSource, out mockTarget);

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Single(mockTarget.Values);
            Assert.Equal("A test value", mockTarget.Values[0]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Equal(0, mockTarget.Values[1]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Equal(0, mockTarget.Values[2]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(4, mockTarget.Values.Count);
            Assert.Equal(0, mockTarget.Values[3]);
        }

        [Fact]
        public void TestDefaultValueIsUsedIfSourceResolutionFails_NullableInt()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(int?), out mockSource, out mockTarget);

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Single(mockTarget.Values);
            Assert.Equal("A test value", mockTarget.Values[0]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(2, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[1]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(3, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[2]);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            Assert.Single(mockValueConverter.ConversionsRequested);
            Assert.Empty(mockValueConverter.ConversionsBackRequested);

            Assert.Equal(4, mockTarget.Values.Count);
            Assert.Null(mockTarget.Values[3]);
        }

        private MvxFullBinding TestSetupCommon(IMvxValueConverter valueConverter, object converterParameter,
                                               Type targetType, out MockSourceBinding mockSource, out MockTargetBinding mockTarget)
        {
            return TestSetupCommon(valueConverter, converterParameter, new { Value = 4 }, targetType, out mockSource, out mockTarget);
        }

        private MvxFullBinding TestSetupCommon(IMvxValueConverter valueConverter, object converterParameter, object fallbackValue,
            Type targetValueType, out MockSourceBinding mockSource, out MockTargetBinding mockTarget)
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
            var bindingDescription = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    Converter = valueConverter,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    SourcePropertyPath = sourceText,
                },
                Mode = MvxBindingMode.TwoWay,
                TargetName = targetName
            };

            mockSource = new MockSourceBinding();
            mockTarget = new MockTargetBinding() { TargetValueType = targetValueType };
            mockTarget.DefaultMode = MvxBindingMode.TwoWay;

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
