// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Moq;
using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Converters;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using Xunit;

namespace MvvmCross.UnitTest.Binding.Bindings
{
    [Collection("MvxTest")]
    public class MvxFullBindingConstructionTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxFullBindingConstructionTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

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

        [Fact]
        public void Test_Creating_A_Binding_Calls_The_Source_And_Target_Factories()
        {
            TestCommon(MvxBindingMode.TwoWay, true, true);
            TestCommon(MvxBindingMode.OneWay, true, false);
            TestCommon(MvxBindingMode.OneWayToSource, false, true);
        }

        private void TestCommon(MvxBindingMode bindingMode, bool expectSourceBinding, bool expectTargetBinding)
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(new InlineMockMainThreadDispatcher());

            var mockSourceBindingFactory = new Mock<IMvxSourceBindingFactory>();
            _fixture.Ioc.RegisterSingleton(mockSourceBindingFactory.Object);

            var mockTargetBindingFactory = new Mock<IMvxTargetBindingFactory>();
            _fixture.Ioc.RegisterSingleton(mockTargetBindingFactory.Object);

            var realSourceStepFactory = new MvxSourceStepFactory();
            realSourceStepFactory.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            _fixture.Ioc.RegisterSingleton<IMvxSourceStepFactory>(realSourceStepFactory);

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
