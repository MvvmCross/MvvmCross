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

namespace MvvmCross.UnitTest.Binding.Bindings
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxFullBindingValueConversionTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxFullBindingValueConversionTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Test]
        public async Task TestConverterIsUsedForConvert()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "Test ConversionResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockSource, out mockTarget);

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("Test ConversionResult");
            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsRequested[0]).IsEqualTo("TryGetValueValue");
        }

        [Test]
        public async Task TestConverterParameterIsUsedForConvert()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "Test ConversionResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockSource, out mockTarget);

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("Test ConversionResult");
            await Assert.That(mockValueConverter.ConversionParameters).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionParameters[0]).IsEqualTo(parameter);
        }

        [Test]
        public async Task TestConverterIsUsedForConvertBack()
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

            await Assert.That(mockSource.ValuesSet).HasSingleItem();
            await Assert.That(mockSource.ValuesSet[0]).IsEqualTo("Test ConversionBackResult");
            await Assert.That(mockValueConverter.ConversionsBackRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested[0]).IsEqualTo(valueChanged);
        }

        [Test]
        public async Task TestConverterParameterIsUsedForConvertBack()
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

            await Assert.That(mockSource.ValuesSet).HasSingleItem();
            await Assert.That(mockSource.ValuesSet[0]).IsEqualTo("Test ConversionBackResult");
            await Assert.That(mockValueConverter.ConversionBackParameters).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionBackParameters[0]).IsEqualTo(parameter);
        }

        [Test]
        public async Task TestTargetTypeIsUsedForConvert()
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

            await Assert.That(mockValueConverter.ConversionTypes.Count).IsEqualTo(2);
            await Assert.That(mockValueConverter.ConversionTypes[0]).IsEqualTo(targetType);
            await Assert.That(mockValueConverter.ConversionTypes[1]).IsEqualTo(targetType);
        }

        [Test]
        public async Task TestSourceTypeIsUsedForConvertBack()
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

            await Assert.That(mockValueConverter.ConversionBackTypes).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionBackTypes[0]).IsEqualTo(aType.GetType());
        }

        [Test]
        public async Task TestFallbackValueIsUsedIfConversionFails()
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

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo(fallback);
        }

        [Test]
        public async Task TestFallbackValueIsUsedIfSourceResolutionFails()
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

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("A test value");

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsEqualTo(fallback);
        }

        [Test]
        public async Task TestDefaultValueIsUsedIfConversionFails_Object()
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

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsNull();

            mockSource.TryGetValueValue = "Fred";
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested.Count).IsEqualTo(2);
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsNull();

            mockSource.TryGetValueValue = "Betty";
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested.Count).IsEqualTo(3);
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsNull();
        }

        [Test]
        public async Task TestDefaultValueIsUsedIfConversionFails_Int()
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

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo(0);

            mockSource.TryGetValueValue = "Fred";
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested.Count).IsEqualTo(2);
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsEqualTo(0);

            mockSource.TryGetValueValue = "Betty";
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested.Count).IsEqualTo(3);
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsEqualTo(0);
        }

        [Test]
        public async Task TestDefaultValueIsUsedIfConversionFails_NullableInt()
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

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsNull();

            mockSource.TryGetValueValue = "Fred";
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested.Count).IsEqualTo(2);
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsNull();

            mockSource.TryGetValueValue = "Betty";
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested.Count).IsEqualTo(3);
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsNull();
        }

        [Test]
        public async Task TestDefaultValueIsUsedIfSourceResolutionFails_Object()
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

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("A test value");

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsNull();

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsNull();

            mockSource.TryGetValueValue = "Fred";
            mockSource.TryGetValueResult = true;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested.Count).IsEqualTo(2);
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(4);
            await Assert.That(mockTarget.Values[3]).IsEqualTo("A test value");
        }

        [Test]
        public async Task TestDefaultValueIsUsedIfSourceResolutionFails_Int()
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

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("A test value");

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsEqualTo(0);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsEqualTo(0);

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(4);
            await Assert.That(mockTarget.Values[3]).IsEqualTo(0);
        }

        [Test]
        public async Task TestDefaultValueIsUsedIfSourceResolutionFails_NullableInt()
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

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values).HasSingleItem();
            await Assert.That(mockTarget.Values[0]).IsEqualTo("A test value");

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(2);
            await Assert.That(mockTarget.Values[1]).IsNull();

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(3);
            await Assert.That(mockTarget.Values[2]).IsNull();

            mockSource.TryGetValueResult = false;
            mockSource.FireSourceChanged();

            await Assert.That(mockValueConverter.ConversionsRequested).HasSingleItem();
            await Assert.That(mockValueConverter.ConversionsBackRequested).IsEmpty();

            await Assert.That(mockTarget.Values.Count).IsEqualTo(4);
            await Assert.That(mockTarget.Values[3]).IsNull();
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
