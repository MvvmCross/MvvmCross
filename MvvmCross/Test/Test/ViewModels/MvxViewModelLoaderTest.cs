// MvxViewModelLoaderTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Moq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Test.Core;
using MvvmCross.Test.Mocks.TestViewModels;
using NUnit.Framework;

namespace MvvmCross.Test.ViewModels
{
    [TestFixture]
    public class MvxViewModelLoaderTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_LoaderForNull()
        {
            ClearAll();

            var request = new MvxViewModelRequest<MvxNullViewModel>(null, null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(null);
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

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(mockCollection.Object);
            var viewModel = loader.LoadViewModel(request, state);

            Assert.AreSame(outViewModel, viewModel);
        }

        [Test]
        public void Test_FailedViewModel()
        {
            ClearAll();

            var mockLocator = new Mock<IMvxViewModelLocator>();
            mockLocator.Setup(
                m => m.Load(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>()))
                       .Throws<MvxException>();

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(mockCollection.Object);
            Assert.Throws<MvxException>(() => {
                var viewModel = loader.LoadViewModel(request, state);
            });
        }

        [Test]
        public void Test_FailedViewModelLocatorCollection()
        {
            ClearAll();

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => null);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(mockCollection.Object);

            Assert.Throws<MvxException>(() => {
                var viewModel = loader.LoadViewModel(request, state);
            });
        }
    }
}