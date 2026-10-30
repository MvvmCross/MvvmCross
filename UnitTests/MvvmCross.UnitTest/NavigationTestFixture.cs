// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Navigation;
using MvvmCross.Tests;
using MvvmCross.ViewModels;
using NSubstitute;

namespace MvvmCross.UnitTest
{
    public class NavigationTestFixture : MvxTestFixture
    {
        protected override void AdditionalSetup()
        {
            var navigationServiceMock = Substitute.For<IMvxNavigationService>();
            Ioc.RegisterSingleton(navigationServiceMock);
            Ioc.RegisterSingleton<IMvxViewModelLocator>(new MvxDefaultViewModelLocator());
        }
    }
}
