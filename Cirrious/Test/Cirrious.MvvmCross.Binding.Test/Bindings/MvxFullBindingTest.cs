// MvxFullBindingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Test.Bindings
{
    [TestFixture]
    public class MvxFullBindingTest : MvxIoCSupportingTest
    {
        public class MockSourceBinding : IMvxSourceBinding
        {
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

            public void FireSourceChanged()
            {
                var handler = Changed;
                handler?.Invoke(this, EventArgs.Empty);
            }

            public event EventHandler Changed;

            public bool TryGetValueResult;
            public object TryGetValueValue;

            public object GetValue()
            {
                if (!TryGetValueResult)
                    return MvxBindingConstant.UnsetValue;

                return TryGetValueValue;
            }
        }

        public class MockTargetBinding : IMvxTargetBinding
        {
            public int DisposeCalled = 0;

            public void Dispose()
            {
                DisposeCalled++;
            }

            public Type TargetType { get; set; }
            public MvxBindingMode DefaultMode { get; set; }

            public int SubscribeToEventsCalled = 0;

            public void SubscribeToEvents()
            {
                SubscribeToEventsCalled++;
            }

            public List<object> Values = new List<object>();

            public void SetValue(object value)
            {
                Values.Add(value);
            }

            public void FireValueChanged(MvxTargetChangedEventArgs args)
            {
                var handler = ValueChanged;
                handler?.Invoke(this, args);
            }

            public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;
        }

        [Test]
        public void TestTwoWayEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.TwoWay, out mockSource, out mockTarget);

            TwoWayAssertions(binding, mockTarget, mockSource);
        }

        [Test]
        public void TestDefaultTwoWayEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.TwoWay, out mockSource, out mockTarget);

            TwoWayAssertions(binding, mockTarget, mockSource);
        }

        private static void TwoWayAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            Assert.AreEqual(1, mockTarget.SubscribeToEventsCalled);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[0]);

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual("SecondValue", mockTarget.Values[1]);

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[2]);

            Assert.AreEqual(0, mockSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(1, mockSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget1", mockSource.ValuesSet[0]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(2, mockSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget2", mockSource.ValuesSet[1]);

            Assert.AreEqual(0, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.AreEqual(1, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(4, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[3]);

            binding.DataContext = new { ignored = 13 };
            Assert.AreEqual(2, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(5, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[4]);

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(6, mockTarget.Values.Count);
            Assert.AreEqual("NewValue", mockTarget.Values[5]);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(3, mockSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget1", mockSource.ValuesSet[2]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(4, mockSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget2", mockSource.ValuesSet[3]);

            binding.Dispose();
            Assert.AreEqual(3, mockSource.DisposeCalled);
            Assert.AreEqual(1, mockTarget.DisposeCalled);
        }

        [Test]
        public void TestOneWayEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.OneWay, out mockSource, out mockTarget);

            OneWayAssertions(binding, mockTarget, mockSource);
        }

        [Test]
        public void TestDefaultOneWayEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneWay, out mockSource, out mockTarget);

            OneWayAssertions(binding, mockTarget, mockSource);
        }

        private static void OneWayAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            Assert.AreEqual(0, mockTarget.SubscribeToEventsCalled);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[0]);

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual("SecondValue", mockTarget.Values[1]);

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[2]);

            Assert.AreEqual(0, mockSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(0, mockSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(0, mockSource.ValuesSet.Count);

            Assert.AreEqual(0, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.AreEqual(1, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(4, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[3]);

            binding.DataContext = new { ignored = 13 };
            Assert.AreEqual(2, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(5, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[4]);

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(6, mockTarget.Values.Count);
            Assert.AreEqual("NewValue", mockTarget.Values[5]);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(0, mockSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(0, mockSource.ValuesSet.Count);

            binding.Dispose();
            Assert.AreEqual(3, mockSource.DisposeCalled);
            Assert.AreEqual(1, mockTarget.DisposeCalled);
        }

        [Test]
        public void TestOneWayToSourceEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.OneWayToSource, out mockSource, out mockTarget);

            OnWayToSourceAssertions(binding, mockTarget, mockSource);
        }

        [Test]
        public void TestDefaultOneWayToSourceEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneWayToSource, out mockSource, out mockTarget);

            OnWayToSourceAssertions(binding, mockTarget, mockSource);
        }

        private static void OnWayToSourceAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            Assert.AreEqual(1, mockTarget.SubscribeToEventsCalled);

            Assert.AreEqual(0, mockTarget.Values.Count);

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(0, mockTarget.Values.Count);

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(0, mockTarget.Values.Count);

            Assert.AreEqual(0, mockSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(1, mockSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget1", mockSource.ValuesSet[0]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(2, mockSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget2", mockSource.ValuesSet[1]);

            Assert.AreEqual(0, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.AreEqual(1, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(0, mockTarget.Values.Count);

            binding.DataContext = new { ignored = 13 };
            Assert.AreEqual(2, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(0, mockTarget.Values.Count);

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(0, mockTarget.Values.Count);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(3, mockSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget1", mockSource.ValuesSet[2]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(4, mockSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget2", mockSource.ValuesSet[3]);

            binding.Dispose();
            Assert.AreEqual(3, mockSource.DisposeCalled);
            Assert.AreEqual(1, mockTarget.DisposeCalled);
        }

        [Test]
        public void TestOneTimeEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.OneTime, out mockSource, out mockTarget);

            OneTimeAssertions(binding, mockTarget, mockSource);
        }

        [Test]
        public void TestDefaultOneTimeEventSubscription()
        {
            MockSourceBinding mockSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneTime, out mockSource, out mockTarget);

            OneTimeAssertions(binding, mockTarget, mockSource);
        }

        private static void OneTimeAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockSourceBinding mockSource)
        {
            Assert.AreEqual(0, mockTarget.SubscribeToEventsCalled);

            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[0]);

            mockSource.TryGetValueValue = "SecondValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(1, mockTarget.Values.Count);

            mockSource.TryGetValueValue = "ThirdValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(1, mockTarget.Values.Count);

            Assert.AreEqual(0, mockSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(0, mockSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(0, mockSource.ValuesSet.Count);

            Assert.AreEqual(0, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.AreEqual(1, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[1]);

            binding.DataContext = new { ignored = 13 };
            Assert.AreEqual(2, mockSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[2]);

            mockSource.TryGetValueValue = "NewValue";
            mockSource.FireSourceChanged();
            Assert.AreEqual(3, mockTarget.Values.Count);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(0, mockSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(0, mockSource.ValuesSet.Count);

            binding.Dispose();
            Assert.AreEqual(3, mockSource.DisposeCalled);
            Assert.AreEqual(1, mockTarget.DisposeCalled);
        }

        private MvxFullBinding TestSetupCommon(MvxBindingMode mvxBindingMode,
                                     out MockSourceBinding mockSource, out MockTargetBinding mockTarget)
        {
            return TestSetupCommon(mvxBindingMode, MvxBindingMode.Default, out mockSource, out mockTarget);
        }

        private MvxFullBinding TestSetupCommon(MvxBindingMode mvxBindingMode, MvxBindingMode defaultMode, out MockSourceBinding mockSource, out MockTargetBinding mockTarget)
        {
            ClearAll();
            MvxBindingSingletonCache.Initialize();

            var mockSourceBindingFactory = new Mock<IMvxSourceBindingFactory>();
            Ioc.RegisterSingleton(mockSourceBindingFactory.Object);

            var mockTargetBindingFactory = new Mock<IMvxTargetBindingFactory>();
            Ioc.RegisterSingleton(mockTargetBindingFactory.Object);

            var realSourceStepFactory = new MvxSourceStepFactory();
            realSourceStepFactory.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            Ioc.RegisterSingleton<IMvxSourceStepFactory>(realSourceStepFactory);

            var sourceText = "sourceText";
            var targetName = "targetName";
            var source = new { Value = 1 };
            var target = new { Value = 2 };
            var converterParameter = new { Value = 3 };
            var fallbackValue = new { Value = 4 };
            IMvxValueConverter converter = null;
            var bindingDescription = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    Converter = converter,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    SourcePropertyPath = sourceText,
                },
                Mode = mvxBindingMode,
                TargetName = targetName
            };

            mockSource = new MockSourceBinding();
            mockTarget = new MockTargetBinding();
            mockTarget.DefaultMode = defaultMode;

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