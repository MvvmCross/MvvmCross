// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Moq;
using MvvmCross.UnitTest.Mocks.ViewModels;
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

            var logProvider = new Mock<ILoggerProvider>();
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
