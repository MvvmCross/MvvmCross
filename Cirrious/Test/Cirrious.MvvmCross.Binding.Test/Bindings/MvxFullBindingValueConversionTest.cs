﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.Bindings
{
    [TestFixture]
    public class MvxFullBindingValueConversionTest : MvxIoCSupportingTest
    {
        public class MockSourceBinding : IMvxSourceBinding
        {
            public MockSourceBinding()
            {
                SourceType = typeof (object);
            }

            public int DisposeCalled = 0;
            public void Dispose()
            {
                DisposeCalled++;
            }

            public Type SourceType { get; set; }

            public List<object> ValuesSet = new List<object>();
            public void SetValue(object value)
            {
                ValuesSet.Add(value);
            }

            public void FireSourceChanged(MvxSourcePropertyBindingEventArgs args)
            {
                var handler = Changed;
                if (handler != null)
                    handler(this, args);
            }

            public event EventHandler<MvxSourcePropertyBindingEventArgs> Changed;

            public bool TryGetValueResult;
            public object TryGetValueValue;

            public bool TryGetValue(out object value)
            {
                value = TryGetValueValue;
                return TryGetValueResult;
            }
        }

        public class MockTargetBinding : IMvxTargetBinding
        {
            public MockTargetBinding()
            {
                TargetType = typeof (object);
            }

            public int DisposeCalled = 0;
            public void Dispose()
            {
                DisposeCalled++;
            }

            public Type TargetType { get; set; }
            public MvxBindingMode DefaultMode { get; set; }

            public List<object> Values = new List<object>();
            public void SetValue(object value)
            {
                Values.Add(value);
            }

            public void FireValueChanged(MvxTargetChangedEventArgs args)
            {
                var handler = ValueChanged;
                if (handler != null)
                    handler(this, args);
            }
            public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;
        }

        public class MockValueConverter : IMvxValueConverter
        {
            public object ConversionResult;
            public bool ThrowOnConversion;

            public List<object> ConversionsRequested = new List<object>();
            public List<object> ConversionParameters = new List<object>();
            public List<Type> ConversionTypes = new List<Type>();

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                ConversionsRequested.Add(value);
                ConversionParameters.Add(parameter);
                ConversionTypes.Add(targetType);
                if (ThrowOnConversion)
                    throw new MvxException("Conversion throw requested");
                return ConversionResult;
            }

            public object ConversionBackResult;
            public bool ThrowOnConversionBack;
            public List<object> ConversionsBackRequested = new List<object>();
            public List<object> ConversionBackParameters = new List<object>();
            public List<Type> ConversionBackTypes = new List<Type>();

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                ConversionsBackRequested.Add(value);
                ConversionBackParameters.Add(parameter);
                ConversionBackTypes.Add(targetType);
                if (ThrowOnConversionBack)
                    throw new MvxException("Conversion throw requested");
                return ConversionBackResult;
            }
        }


        [Test]
        public void TestConverterIsUsedForConvert()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
                {
                    ConversionResult = "Test ConversionResult",
                };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, out mockSource, out mockTarget);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("Test ConversionResult", mockTarget.Values[0]);
            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual("TryGetValueValue", mockValueConverter.ConversionsRequested[0]);
        }

        [Test]
        public void TestConverterParameterIsUsedForConvert()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "Test ConversionResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, out mockSource, out mockTarget);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("Test ConversionResult", mockTarget.Values[0]);
            Assert.AreEqual(1, mockValueConverter.ConversionParameters.Count);
            Assert.AreEqual(parameter, mockValueConverter.ConversionParameters[0]);
        }

        [Test]
        public void TestConverterIsUsedForConvertBack()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionBackResult = "Test ConversionBackResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, out mockSource, out mockTarget);

            var valueChanged = new { Hello = 34 };
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs(valueChanged));

            Assert.AreEqual(1, mockSource.ValuesSet.Count);
            Assert.AreEqual("Test ConversionBackResult", mockSource.ValuesSet[0]);
            Assert.AreEqual(1, mockValueConverter.ConversionsBackRequested.Count);
            Assert.AreEqual(valueChanged, mockValueConverter.ConversionsBackRequested[0]);
        }

        [Test]
        public void TestConverterParameterIsUsedForConvertBack()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionBackResult = "Test ConversionBackResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, out mockSource, out mockTarget);

            var valueChanged = new {Hello = 34};
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs(valueChanged));

            Assert.AreEqual(1, mockSource.ValuesSet.Count);
            Assert.AreEqual("Test ConversionBackResult", mockSource.ValuesSet[0]);
            Assert.AreEqual(1, mockValueConverter.ConversionBackParameters.Count);
            Assert.AreEqual(parameter, mockValueConverter.ConversionBackParameters[0]);
        }

        [Test]
        public void TestTargetTypeIsUsedForConvert()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, out mockSource, out mockTarget);

            var targetType = new { Foo = 23 }.GetType();
            mockTarget.TargetType = targetType;

            mockSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "new value"));

            Assert.AreEqual(2, mockValueConverter.ConversionTypes.Count);
            Assert.AreEqual(typeof(object), mockValueConverter.ConversionTypes[0]);
            Assert.AreEqual(targetType, mockValueConverter.ConversionTypes[1]);
        }

        [Test]
        public void TestSourceTypeIsUsedForConvertBack()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, out mockSource, out mockTarget);

            var aType = new { Hello = 34 };
            mockSource.SourceType = aType.GetType();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("Ignored"));

            Assert.AreEqual(1, mockValueConverter.ConversionBackTypes.Count);
            Assert.AreEqual(aType.GetType(), mockValueConverter.ConversionBackTypes[0]);
        }

        [Test]
        public void TestFallbackValueIsUsedIfConversionFails()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            var fallback = new {Fred = "Not Barney"};
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, out mockSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual(fallback, mockTarget.Values[0]);
        }

        [Test]
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
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, out mockSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("A test value", mockTarget.Values[0]);

            mockSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, 42));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(fallback, mockTarget.Values[1]);
        }

        [Test]
        public void TestDefaultValueIsUsedIfConversionFails()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, out mockSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[0]);

            mockTarget.TargetType = typeof (int);
            mockSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "Fred"));

            Assert.AreEqual(2, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(0, mockTarget.Values[1]);

            mockTarget.TargetType = typeof(int?);
            mockSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "Betty"));

            Assert.AreEqual(3, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[2]);
        }

        [Test]
        public void TestDefaultValueIsUsedIfSourceResolutionFails()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, out mockSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("A test value", mockTarget.Values[0]);

            mockSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, 42));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[1]);

            mockTarget.TargetType = typeof(int);
            mockSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, "Fred"));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual(0, mockTarget.Values[2]);

            mockTarget.TargetType = typeof(int?);
            mockSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, "Betty"));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(4, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[3]);
        }


        private MvxFullBinding TestSetupCommon(IMvxValueConverter valueConverter, object converterParameter,
                                               out MockSourceBinding mockSource, out MockTargetBinding mockTarget)
        {
            return TestSetupCommon(valueConverter, converterParameter, new {Value = 4}, out mockSource, out mockTarget);
        }

        private MvxFullBinding TestSetupCommon(IMvxValueConverter valueConverter, object converterParameter, object fallbackValue, out MockSourceBinding mockSource, out MockTargetBinding mockTarget)
        {
            ClearAll();
            MvxBindingSingletonCache.Initialise();

            var mockSourceBindingFactory = new Mock<IMvxSourceBindingFactory>();
            Ioc.RegisterSingleton(mockSourceBindingFactory.Object);

            var mockTargetBindingFactory = new Mock<IMvxTargetBindingFactory>();
            Ioc.RegisterSingleton(mockTargetBindingFactory.Object);

            var sourceText = "sourceText";
            var targetName = "targetName";
            var source = new {Value = 1};
            var target = new {Value = 2};
            var bindingDescription = new MvxBindingDescription
                {
                    Converter = valueConverter,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    Mode = MvxBindingMode.TwoWay,
                    SourcePropertyPath = sourceText,
                    TargetName = targetName
                };

            mockSource = new MockSourceBinding();
            mockTarget = new MockTargetBinding();
            mockTarget.DefaultMode = MvxBindingMode.TwoWay;

            var localSource = mockSource;
            mockSourceBindingFactory
                .Setup(x => x.CreateBinding(It.IsAny<object>(), It.Is<string>(s => s == sourceText)))
                .Returns((object a, string b) => localSource);
            var localTarget = mockTarget;
            mockTargetBindingFactory
                .Setup(x => x.CreateBinding(It.IsAny<object>(), It.Is<string>(s => s == targetName)))
                .Returns((object a, string b) => localTarget);

            mockSource.TryGetValueResult = true;
            mockSource.TryGetValueValue = "TryGetValueValue";

            var request = new MvxBindingRequest(source, target, bindingDescription);
            var toTest = new MvxFullBinding(request);
            return toTest;
        }
    }
}