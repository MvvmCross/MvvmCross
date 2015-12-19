// MvxViewModelLoaderTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Test.Core;
    using MvvmCross.Test.Mocks.TestViewModels;

    using NUnit.Framework;

    [TestFixture]
    public class MvxViewModelLoaderTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_LoaderForNull()
        {
            ClearAll();

            var request = new MvxViewModelRequest<MvxNullViewModel>(null, null, MvxRequestedBy.UserAction);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader();
            var viewModel = loader.LoadViewModel(request, state);

            Assert.IsInstanceOf<MvxNullViewModel>(viewModel);
        }

        [Test]
        public void Test_NormalViewModel()
        {
            ClearAll();

            IMvxViewModel outViewModel = new Test2ViewModel();

            var mockLocator = new Mock<IMvxViewModelLocator>();
            mockLocator.Setup(
                m => m.Load(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>()))
                       .Returns(() => outViewModel);

            var mockCollection = new Moq.Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            Ioc.RegisterSingleton(mockCollection.Object);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null,
                                                                  MvxRequestedBy.UserAction);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader();
            var viewModel = loader.LoadViewModel(request, state);

            Assert.AreSame(outViewModel, viewModel);
            Assert.AreEqual(MvxRequestedBy.UserAction, viewModel.RequestedBy);
        }

        [Test]
        [ExpectedException(typeof(MvxException))]
        public void Test_FailedViewModel()
        {
            ClearAll();

            var mockLocator = new Mock<IMvxViewModelLocator>();
            mockLocator.Setup(
                m => m.Load(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>()))
                       .Throws<MvxException>();

            var mockCollection = new Moq.Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            Ioc.RegisterSingleton(mockCollection.Object);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null,
                                                                  MvxRequestedBy.UserAction);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader();
            var viewModel = loader.LoadViewModel(request, state);

            Assert.Fail("We should never reach this line");
        }

        [Test]
        [ExpectedException(typeof(MvxException))]
        public void Test_FailedViewModelLocatorCollection()
        {
            ClearAll();

            var mockCollection = new Moq.Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => null);

            Ioc.RegisterSingleton(mockCollection.Object);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null,
                                                                  MvxRequestedBy.UserAction);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader();
            var viewModel = loader.LoadViewModel(request, state);

            Assert.Fail("We should never reach this line");
        }
    }
}