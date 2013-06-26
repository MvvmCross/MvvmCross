// MvxFullBindingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Converters;
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
    public class MvxFullBindingTest : MvxIoCSupportingTest
    {
        public class MockPathSourceBinding : IMvxPathSourceBinding
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

        [Test]
        public void TestTwoWayEventSubscription()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.TwoWay, out mockPathSource, out mockTarget);

            TwoWayAssertions(binding, mockTarget, mockPathSource);
        }

        [Test]
        public void TestDefaultTwoWayEventSubscription()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.TwoWay, out mockPathSource, out mockTarget);

            TwoWayAssertions(binding, mockTarget, mockPathSource);
        }

        private static void TwoWayAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockPathSourceBinding mockPathSource)
        {
            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "SecondValue"));
            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual("SecondValue", mockTarget.Values[1]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "ThirdValue"));
            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[2]);

            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(1, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget1", mockPathSource.ValuesSet[0]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(2, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget2", mockPathSource.ValuesSet[1]);

            Assert.AreEqual(0, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.AreEqual(1, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(4, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[3]);

            binding.DataContext = new { ignored = 13 };
            Assert.AreEqual(2, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(5, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[4]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "NewValue"));
            Assert.AreEqual(6, mockTarget.Values.Count);
            Assert.AreEqual("NewValue", mockTarget.Values[5]);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(3, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget1", mockPathSource.ValuesSet[2]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(4, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget2", mockPathSource.ValuesSet[3]);

            binding.Dispose();
            Assert.AreEqual(3, mockPathSource.DisposeCalled);
            Assert.AreEqual(1, mockTarget.DisposeCalled);
        }

        [Test]
        public void TestOneWayEventSubscription()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.OneWay, out mockPathSource, out mockTarget);

            OneWayAssertions(binding, mockTarget, mockPathSource);
        }

        [Test]
        public void TestDefaultOneWayEventSubscription()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneWay, out mockPathSource, out mockTarget);

            OneWayAssertions(binding, mockTarget, mockPathSource);
        }

        private static void OneWayAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockPathSourceBinding mockPathSource)
        {
            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "SecondValue"));
            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual("SecondValue", mockTarget.Values[1]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "ThirdValue"));
            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual("ThirdValue", mockTarget.Values[2]);

            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);

            Assert.AreEqual(0, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.AreEqual(1, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(4, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[3]);

            binding.DataContext = new { ignored = 13 };
            Assert.AreEqual(2, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(5, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[4]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "NewValue"));
            Assert.AreEqual(6, mockTarget.Values.Count);
            Assert.AreEqual("NewValue", mockTarget.Values[5]);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);

            binding.Dispose();
            Assert.AreEqual(3, mockPathSource.DisposeCalled);
            Assert.AreEqual(1, mockTarget.DisposeCalled);
        }

        [Test]
        public void TestOneWayToSourceEventSubscription()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.OneWayToSource, out mockPathSource, out mockTarget);

            OnWayToSourceAssertions(binding, mockTarget, mockPathSource);
        }

        [Test]
        public void TestDefaultOneWayToSourceEventSubscription()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneWayToSource, out mockPathSource, out mockTarget);

            OnWayToSourceAssertions(binding, mockTarget, mockPathSource);
        }

        private static void OnWayToSourceAssertions(MvxFullBinding binding, MockTargetBinding mockTarget, MockPathSourceBinding mockPathSource)
        {
            Assert.AreEqual(0, mockTarget.Values.Count);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "SecondValue"));
            Assert.AreEqual(0, mockTarget.Values.Count);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "ThirdValue"));
            Assert.AreEqual(0, mockTarget.Values.Count);

            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(1, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget1", mockPathSource.ValuesSet[0]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(2, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget2", mockPathSource.ValuesSet[1]);

            Assert.AreEqual(0, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.AreEqual(1, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(0, mockTarget.Values.Count);

            binding.DataContext = new { ignored = 13 };
            Assert.AreEqual(2, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(0, mockTarget.Values.Count);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "NewValue"));
            Assert.AreEqual(0, mockTarget.Values.Count);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(3, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget1", mockPathSource.ValuesSet[2]);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(4, mockPathSource.ValuesSet.Count);
            Assert.AreEqual("FromTarget2", mockPathSource.ValuesSet[3]);

            binding.Dispose();
            Assert.AreEqual(3, mockPathSource.DisposeCalled);
            Assert.AreEqual(1, mockTarget.DisposeCalled);
        }

        [Test]
        public void TestOneTimeEventSubscription()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.OneTime, out mockPathSource, out mockTarget);

            OneTimeAsserrtions(binding, mockTarget, mockPathSource);
        }

        [Test]
        public void TestDefaultOneTimeEventSubscription()
        {
            MockPathSourceBinding mockPathSource;
            MockTargetBinding mockTarget;
            var binding = TestSetupCommon(MvxBindingMode.Default, MvxBindingMode.OneTime, out mockPathSource, out mockTarget);

            OneTimeAsserrtions(binding, mockTarget, mockPathSource);
        }

        private static void OneTimeAsserrtions(MvxFullBinding binding, MockTargetBinding mockTarget, MockPathSourceBinding mockPathSource)
        {
            Assert.AreEqual(1, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[0]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "SecondValue"));
            Assert.AreEqual(1, mockTarget.Values.Count);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "ThirdValue"));
            Assert.AreEqual(1, mockTarget.Values.Count);

            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);


            Assert.AreEqual(0, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            binding.DataContext = new { ignored = 12 };
            Assert.AreEqual(1, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(2, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[1]);

            binding.DataContext = new { ignored = 13 };
            Assert.AreEqual(2, mockPathSource.DisposeCalled);
            Assert.AreEqual(0, mockTarget.DisposeCalled);

            Assert.AreEqual(3, mockTarget.Values.Count);
            Assert.AreEqual("TryGetValueValue", mockTarget.Values[2]);

            mockPathSource.FireSourceChanged(new MvxSourcePropertyBindingEventArgs(true, "NewValue"));
            Assert.AreEqual(3, mockTarget.Values.Count);

            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget1"));
            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);
            mockTarget.FireValueChanged(new MvxTargetChangedEventArgs("FromTarget2"));
            Assert.AreEqual(0, mockPathSource.ValuesSet.Count);

            binding.Dispose();
            Assert.AreEqual(3, mockPathSource.DisposeCalled);
            Assert.AreEqual(1, mockTarget.DisposeCalled);
        }

        private MvxFullBinding TestSetupCommon(MvxBindingMode mvxBindingMode,
                                     out MockPathSourceBinding mockPathSource, out MockTargetBinding mockTarget)
        {
            return TestSetupCommon(mvxBindingMode, MvxBindingMode.Default, out mockPathSource, out mockTarget);
        }

        private MvxFullBinding TestSetupCommon(MvxBindingMode mvxBindingMode, MvxBindingMode defaultMode, out MockPathSourceBinding mockPathSource, out MockTargetBinding mockTarget)
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

            mockPathSource = new MockPathSourceBinding();
            mockTarget = new MockTargetBinding();
            mockTarget.DefaultMode = defaultMode;

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