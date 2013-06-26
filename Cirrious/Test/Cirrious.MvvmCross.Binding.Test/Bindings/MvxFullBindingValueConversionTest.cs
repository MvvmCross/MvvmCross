using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Bindings.PathSource;
using Cirrious.MvvmCross.Binding.Bindings.PathSource.Construction;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
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
        public class MockPathSourceBinding : IMvxPathSourceBinding
        {
            public MockPathSourceBinding()
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
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
                {
                    ConversionResult = "Test ConversionResult",
                };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("Test ConversionResult", mockTarget.Values[0]);
            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual("TryGetValueValue", mockValueConverter.ConversionsRequested[0]);
        }

        [Test]
        public void TestConverterParameterIsUsedForConvert()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "Test ConversionResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("Test ConversionResult", mockTarget.Values[0]);
            Assert.AreEqual(1, mockValueConverter.ConversionParameters.Count);
            Assert.AreEqual(parameter, mockValueConverter.ConversionParameters[0]);
        }

        [Test]
        public void TestConverterIsUsedForConvertBack()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionBackResult = "Test ConversionBackResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockPathSource, out mockTarget);

            var valueChanged = new { Hello = 34 };
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs(valueChanged));

            Assert.AreEqual(1, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("Test ConversionBackResult", mockPathSource.ValuesSet[0]);
            Assert.AreEqual(1, mockValueConverter.ConversionsBackRequested.Count);
            Assert.AreEqual(valueChanged, mockValueConverter.ConversionsBackRequested[0]);
        }

        [Test]
        public void TestConverterParameterIsUsedForConvertBack()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionBackResult = "Test ConversionBackResult",
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockPathSource, out mockTarget);

            var valueChanged = new {Hello = 34};
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs(valueChanged));

            Assert.AreEqual(1, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("Test ConversionBackResult", mockPathSource.ValuesSet[0]);
            Assert.AreEqual(1, mockValueConverter.ConversionBackParameters.Count);
            Assert.AreEqual(parameter, mockValueConverter.ConversionBackParameters[0]);
        }

        [Test]
        public void TestTargetTypeIsUsedForConvert()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
            };
            var parameter = new { Ignored = 12 };
            var targetType = new { Foo = 23 }.GetType();
            var binding = TestSetupCommon(mockValueConverter, parameter, targetType, out mockPathSource, out mockTarget);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "new value"));

            Assert.AreEqual(2, mockValueConverter.ConversionTypes.Count);
            Assert.AreEqual(targetType, mockValueConverter.ConversionTypes[0]);
            Assert.AreEqual(targetType, mockValueConverter.ConversionTypes[1]);
        }

        [Test]
        public void TestSourceTypeIsUsedForConvertBack()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
            };
            var parameter = new { Ignored = 12 };
            var binding = TestSetupCommon(mockValueConverter, parameter, typeof(object), out mockPathSource, out mockTarget);

            var aType = new { Hello = 34 };
            mockPathSource.SourceType = aType.GetType();
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("Ignored"));

            Assert.AreEqual(1, mockValueConverter.ConversionBackTypes.Count);
            Assert.AreEqual(aType.GetType(), mockValueConverter.ConversionBackTypes[0]);
        }

        [Test]
        public void TestFallbackValueIsUsedIfConversionFails()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            var fallback = new {Fred = "Not Barney"};
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(object), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual(fallback, mockTarget.Values[0]);
        }

        [Test]
        public void TestFallbackValueIsUsedIfSourceResolutionFails()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            var fallback = new { Fred = "Not Barney" };
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(object), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("A test value", mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, 42));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(fallback, mockTarget.Values[1]);
        }

        [Test]
        public void TestDefaultValueIsUsedIfConversionFails_Object()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof (object), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "Fred"));

            Assert.AreEqual(2, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[1]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "Betty"));

            Assert.AreEqual(3, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[2]);
        }

        [Test]
        public void TestDefaultValueIsUsedIfConversionFails_Int()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(int), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual(0, mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "Fred"));

            Assert.AreEqual(2, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(0, mockTarget.Values[1]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "Betty"));

            Assert.AreEqual(3, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual(0, mockTarget.Values[2]);
        }

        [Test]
        public void TestDefaultValueIsUsedIfConversionFails_NullableInt()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ThrowOnConversion = true
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(int?), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "Fred"));

            Assert.AreEqual(2, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[1]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "Betty"));

            Assert.AreEqual(3, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[2]);
        }

        [Test]
        public void TestDefaultValueIsUsedIfSourceResolutionFails_Object()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(object), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("A test value", mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, 42));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[1]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, "Fred"));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[2]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, "Betty"));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(4, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[3]);
        }

        [Test]
        public void TestDefaultValueIsUsedIfSourceResolutionFails_Int()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(int), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("A test value", mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, 42));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(0, mockTarget.Values[1]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, "Fred"));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual(0, mockTarget.Values[2]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, "Betty"));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(4, mockTarget.Values.Count);
            Assert.AreEqual(0, mockTarget.Values[3]);
        }

        [Test]
        public void TestDefaultValueIsUsedIfSourceResolutionFails_NullableInt()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var mockValueConverter = new MockValueConverter()
            {
                ConversionResult = "A test value"
            };
            var parameter = new { Ignored = 12 };
            object fallback = null;
            var binding = TestSetupCommon(mockValueConverter, parameter, fallback, typeof(int?), out mockPathSource, out mockTarget);

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("A test value", mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, 42));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[1]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, "Fred"));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[2]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(false, "Betty"));

            Assert.AreEqual(1, mockValueConverter.ConversionsRequested.Count);
            Assert.AreEqual(0, mockValueConverter.ConversionsBackRequested.Count);

            Assert.AreEqual(4, mockTarget.Values.Count);
            Assert.AreEqual(null, mockTarget.Values[3]);
        }

        private MvxFullBinding TestSetupCommon(IMvxValueConverter valueConverter, object converterParameter,
                                               Type targetType, out MockPathSourceBinding mockPathSource, out MockTargetBinding mockTarget)
        {
            return TestSetupCommon(valueConverter, converterParameter, new {Value = 4}, targetType, out mockPathSource, out mockTarget);
        }

        private MvxFullBinding TestSetupCommon(IMvxValueConverter valueConverter, object converterParameter, object fallbackValue,
            Type targetType, out MockPathSourceBinding mockPathSource, out MockTargetBinding mockTarget)
        {
            ClearAll();
            MvxBindingSingletonCache.Initialise();

            var mockSourceBindingFactory = new Mock<IMvxPathSourceBindingFactory>();
            Ioc.RegisterSingleton(mockSourceBindingFactory.Object);

            var mockTargetBindingFactory = new Mock<IMvxTargetBindingFactory>();
            Ioc.RegisterSingleton(mockTargetBindingFactory.Object);

            var realSourceStepFactory = new MvxSourceStepFactory();
            realSourceStepFactory.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            Ioc.RegisterSingleton<IMvxSourceStepFactory>(realSourceStepFactory);

            var sourceText = "sourceText";
            var targetName = "targetName";
            var source = new {Value = 1};
            var target = new {Value = 2};
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

            mockPathSource = new MockPathSourceBinding();
            mockTarget = new MockTargetBinding() {TargetType = targetType};
            mockTarget.DefaultMode = MvxBindingMode.TwoWay;

            var localSource = mockPathSource;
            mockSourceBindingFactory
                .Setup(x => x.CreateBinding(It.IsAny<object>(), It.Is<string>(s => s == sourceText)))
                .Returns((object a, string b) => localSource);
            var localTarget = mockTarget;
            mockTargetBindingFactory
                .Setup(x => x.CreateBinding(It.IsAny<object>(), It.Is<string>(s => s == targetName)))
                .Returns((object a, string b) => localTarget);

            mockPathSource.TryGetValueResult = true;
            mockPathSource.TryGetValueValue = "TryGetValueValue";

            var request = new MvxBindingRequest(source, target, bindingDescription);
            var toTest = new MvxFullBinding(request);
            return toTest;
        }
    }
}