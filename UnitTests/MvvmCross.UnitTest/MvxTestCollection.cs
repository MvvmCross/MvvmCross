// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Moq;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest
{
    [CollectionDefinition("MvxTest")]
    public class MvxTestCollection : ICollectionFixture<NavigationTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class NavigationTestFixture : MvxTestFixture
    {
        protected override void AdditionalSetup()
        {
            var navigationServiceMock = new Mock<IMvxNavigationService>();
            Ioc.RegisterSingleton(navigationServiceMock.Object);
            Ioc.RegisterSingleton(new MvxDefaultViewModelLocator(navigationServiceMock.Object));
        }
    }
}
