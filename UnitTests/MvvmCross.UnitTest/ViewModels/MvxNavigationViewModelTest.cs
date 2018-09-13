// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Moq;
using MvvmCross.Core;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.UnitTest.Mocks.ViewModels;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxNavigationViewModelTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxNavigationViewModelTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;

            var logProvider = new Mock<IMvxLogProvider>();
            logProvider.Setup(
                l => l.GetLogFor(It.IsAny<Type>()))
                      .Returns(() =>
                      {
                          var vm = new Mock<IMvxLog>();
                          return vm.Object;
                      });
            _fixture.Ioc.RegisterSingleton(logProvider.Object);

            fixture.Ioc.RegisterType<NavigationTestViewModel, NavigationTestViewModel>();
        }

        [Fact]
        public void Test_RoundTrip()
        {
            var navViewModel = _fixture.Ioc.Resolve<NavigationTestViewModel>();
            Assert.NotNull(navViewModel);
            Assert.NotNull(navViewModel.NavService);
            Assert.NotNull(navViewModel.LoggingProvider);
            Assert.NotNull(navViewModel.ViewModelLog);
        }

    }
}
