// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Moq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Test.Core;
using MvvmCross.Test.Mocks.TestViewModels;
using Xunit;

namespace MvvmCross.Test.ViewModels
{
    
    public class MvxViewModelLoaderTest : MvxIoCSupportingTest
    {
        [Fact]
        public void Test_LoaderForNull()
        {
            ClearAll();

            var request = new MvxViewModelRequest<MvxNullViewModel>(null, null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(null);
            var viewModel = loader.LoadViewModel(request, state);

            Assert.IsInstanceOf<MvxNullViewModel>(viewModel);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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