// MvxFullBindingConstructionTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Test.Bindings
{
    using System;

    using Moq;

    using MvvmCross.Platform.Converters;

    using NUnit.Framework;

    [TestFixture]
    public class MvxFullBindingConstructionTest : MvxIoCSupportingTest
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
                if (this.DisposeCalled)
                {
                    throw new Exception("Multiple dispose calls seen");
                }

                this.DisposeCalled = true;
                if (isDisposing)
                {
                    this.DisposeCalledWithIsDisposing = true;
                }

                base.Dispose(isDisposing);
            }
        }

        [Test]
        public void Test_Creating_A_Binding_Calls_The_Source_And_Target_Factories()
        {
            this.TestCommon(MvxBindingMode.TwoWay, true, true);
            this.TestCommon(MvxBindingMode.OneWay, true, false);
            this.TestCommon(MvxBindingMode.OneWayToSource, false, true);
        }

        private void TestCommon(MvxBindingMode bindingMode, bool expectSourceBinding, bool expectTargetBinding)
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
            var converter = new Mock<IMvxValueConverter>();
            var bindingDescription = new MvxBindingDescription
            {
                Source = new MvxPathSourceStepDescription()
                {
                    Converter = converter.Object,
                    ConverterParameter = converterParameter,
                    FallbackValue = fallbackValue,
                    SourcePropertyPath = sourceText,
                },
                Mode = bindingMode,
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
                .Verify(x => x.CreateBinding(It.Is<object>(s => s == source), It.Is<string>(s => s == sourceText)),
                        Times.Once());

            //var targetBindingTimes = expectSourceBinding ? Times.Once() : Times.Never();
            //mockTargetBinding.Verify(x => x.ValueChanged += It.IsAny<EventHandler<MvxTargetChangedEventArgs>>(), targetBindingTimes);
            mockTargetBindingFactory
                .Verify(x => x.CreateBinding(It.Is<object>(s => s == target), It.Is<string>(s => s == targetName)),
                        Times.Once());
        }
    }
}