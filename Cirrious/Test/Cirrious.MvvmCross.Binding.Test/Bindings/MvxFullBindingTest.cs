﻿using System;
using Cirrious.CrossCore.Interfaces.Converters;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.Bindings
{
    [TestFixture]
    public class MvxFullBindingTest : BaseIoCSupportingTest
    {
        public class MyBinding : MvxFullBinding
        {
            public MyBinding(MvxBindingRequest bindingRequest)
                : base(bindingRequest)
            {
            }

            public bool GetNeedToObserveSourceChanges()
            {
                return NeedToObserveSourceChanges;
            }

            public bool GetNeedToObserveTargetChanges()
            {
                return NeedToObserveTargetChanges;
            }

            public bool GetNeedToUpdateTargetOnBind()
            {
                return NeedToUpdateTargetOnBind;
            }

            public bool DisposeCalled { get; private set; }
            public bool DisposeCalledWithIsDisposing { get; private set; }

            protected override void Dispose(bool isDisposing)
            {
                if (DisposeCalled)
                {
                    throw new Exception("Multiple dispose calls seen");
                }

                DisposeCalled = true;
                if (isDisposing)
                {
                    DisposeCalledWithIsDisposing = true;
                }

                base.Dispose(isDisposing);
            }
        }

        [Test]
        public void Test_Creating_A_Binding_Calls_The_Source_And_Target_Factories()
        {
            TestCommon(MvxBindingMode.TwoWay, true, true);
            TestCommon(MvxBindingMode.OneWay, true, false);
            TestCommon(MvxBindingMode.OneWayToSource, false, true);
        }

        // TODO: need to test other things like:
        // - event subscription (hard to do through Moq)
        // - use of valueConverter
        // - one time subscription
        // - fallback values
        // - setup again on DataContext change
        // - dispose mechanism

        private void TestCommon(MvxBindingMode bindingMode, bool expectSourceBinding, bool expectTargetBinding)
        {
            var mockSourceBindingFactory = new Mock<IMvxSourceBindingFactory>();
            Ioc.RegisterServiceInstance<IMvxSourceBindingFactory>(mockSourceBindingFactory.Object);

            var mockTargetBindingFactory = new Mock<IMvxTargetBindingFactory>();
            Ioc.RegisterServiceInstance<IMvxTargetBindingFactory>(mockTargetBindingFactory.Object);

            var sourceText = "sourceText";
            var targetName = "targetName";
            var source = new {Value = 1};
            var target = new {Value = 2};
            var converterParameter = new {Value = 3};
            var fallbackValue = new {Value = 4};
            var converter = new Mock<IMvxValueConverter>();
            var bindingDescription = new MvxBindingDescription()
                {
                    Converter = converter.Object,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    Mode = bindingMode,
                    SourcePropertyPath = sourceText,
                    TargetName = targetName
                };

            var mockSourceBinding = new Mock<IMvxSourceBinding>();
            var mockTargetBinding = new Mock<IMvxTargetBinding>();

            mockSourceBindingFactory
                .Setup(x => x.CreateBinding(It.Is<object>(s => s == source), It.Is<string>(s => s == sourceText)))
                .Returns((object a, string b) => mockSourceBinding.Object);
            mockTargetBindingFactory
                .Setup(x => x.CreateBinding(It.Is<object>(s => s == target), It.Is<string>(s => s == targetName)))
                .Returns((object a, string b) => mockTargetBinding.Object);

            var request = new MvxBindingRequest(source, target, bindingDescription);

            var toTest = new MvxFullBinding(request);

            //var sourceBindingTimes = expectSourceBinding ? Times.Once() : Times.Never();
            //mockSourceBinding.Verify(x => x.Changed += It.IsAny<EventHandler<MvxSourcePropertyBindingEventArgs>>(), sourceBindingTimes);
            mockSourceBindingFactory
                .Verify(x => x.CreateBinding(It.Is<object>(s => s == source), It.Is<string>(s => s == sourceText)), Times.Once());

            //var targetBindingTimes = expectSourceBinding ? Times.Once() : Times.Never();
            //mockTargetBinding.Verify(x => x.ValueChanged += It.IsAny<EventHandler<MvxTargetChangedEventArgs>>(), targetBindingTimes);
            mockTargetBindingFactory
                .Verify(x => x.CreateBinding(It.Is<object>(s => s == target), It.Is<string>(s => s == targetName)), Times.Once());
        }
    }
}
