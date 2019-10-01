// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxDefaultViewModelLocatorTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxDefaultViewModelLocatorTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_NoReloadState()
        {
            _fixture.ClearAll();

            _fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var testThing = new MockTestThing();
            _fixture.Ioc.RegisterSingleton<ITestThing>(testThing);

            var testObject = new BundleObject
            {
                TheBool1 = false,
                TheBool2 = true,
                TheGuid1 = Guid.NewGuid(),
                TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                TheInt1 = 123,
                TheInt2 = 456,
                TheString1 = "Hello World",
                TheString2 = null
            };
            var bundle = new MvxBundle();
            bundle.Write(testObject);

            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();
            var toTest = new MvxDefaultViewModelLocator(navigationService);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);

            IMvxViewModel viewModel = toTest.Load(typeof(Test1ViewModel), bundle, null, args);

            Assert.NotNull(viewModel);
            var typedViewModel = (Test1ViewModel)viewModel;
            Assert.Equal(bundle, typedViewModel.BundleInit);
            Assert.Null(typedViewModel.BundleState);
            Assert.Equal(testThing, typedViewModel.Thing);
            Assert.Equal(testObject, typedViewModel.TheInitBundleSet);
            Assert.Null(typedViewModel.TheReloadBundleSet);
            Assert.Equal(testObject.TheGuid1, typedViewModel.TheInitGuid1Set);
            Assert.Equal(testObject.TheGuid2, typedViewModel.TheInitGuid2Set);
            Assert.Equal(testObject.TheString1, typedViewModel.TheInitString1Set);
            Assert.Equal(Guid.Empty, typedViewModel.TheReloadGuid1Set);
            Assert.Equal(Guid.Empty, typedViewModel.TheReloadGuid2Set);
            Assert.Null(typedViewModel.TheReloadString1Set);
            Assert.True(typedViewModel.StartCalled);
        }

        [Fact]
        public void Test_WithReloadState()
        {
            _fixture.ClearAll();

            _fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var testThing = new MockTestThing();
            _fixture.Ioc.RegisterSingleton<ITestThing>(testThing);

            var initBundleObject = new BundleObject
            {
                TheBool1 = false,
                TheBool2 = true,
                TheGuid1 = Guid.NewGuid(),
                TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                TheInt1 = 123,
                TheInt2 = 456,
                TheString1 = "Hello World",
                TheString2 = null
            };
            var initBundle = new MvxBundle();
            initBundle.Write(initBundleObject);

            var reloadBundleObject = new BundleObject
            {
                TheBool1 = true,
                TheBool2 = true,
                TheGuid1 = Guid.NewGuid(),
                TheGuid2 = new Guid(1123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                TheInt1 = 1234,
                TheInt2 = 4567,
                TheString1 = "Foo Bar",
                TheString2 = null
            };
            var reloadBundle = new MvxBundle();
            reloadBundle.Write(reloadBundleObject);

            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();
            var toTest = new MvxDefaultViewModelLocator(navigationService);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            IMvxViewModel viewModel = toTest.Load(typeof(Test1ViewModel), initBundle, reloadBundle, args);

            Assert.NotNull(viewModel);
            var typedViewModel = (Test1ViewModel)viewModel;
            Assert.Equal(initBundle, typedViewModel.BundleInit);
            Assert.Equal(reloadBundle, typedViewModel.BundleState);
            Assert.Equal(testThing, typedViewModel.Thing);
            Assert.Equal(initBundleObject, typedViewModel.TheInitBundleSet);
            Assert.Equal(reloadBundleObject, typedViewModel.TheReloadBundleSet);
            Assert.Equal(initBundleObject.TheGuid1, typedViewModel.TheInitGuid1Set);
            Assert.Equal(initBundleObject.TheGuid2, typedViewModel.TheInitGuid2Set);
            Assert.Equal(initBundleObject.TheString1, typedViewModel.TheInitString1Set);
            Assert.Equal(reloadBundleObject.TheGuid1, typedViewModel.TheReloadGuid1Set);
            Assert.Equal(reloadBundleObject.TheGuid2, typedViewModel.TheReloadGuid2Set);
            Assert.Equal(reloadBundleObject.TheString1, typedViewModel.TheReloadString1Set);
            Assert.True(typedViewModel.StartCalled);
        }

        [Fact]
        public void Test_MissingDependency()
        {
            _fixture.ClearAll();

            var bundle = new MvxBundle();

            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();
            var toTest = new MvxDefaultViewModelLocator(navigationService);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            Assert.Throws<MvxException>(() =>
            {
                IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null, args);
            });
        }

        [Fact]
        public void Test_FailingDependency()
        {
            _fixture.ClearAll();

            _fixture.Ioc.RegisterSingleton<ITestThing>(() => new FailingMockTestThing());

            var bundle = new MvxBundle();

            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();
            var toTest = new MvxDefaultViewModelLocator(navigationService);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);

            Assert.Throws<MvxException>(() =>
            {
                IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null, args);
            });
        }

        [Fact]
        public void Test_FailingInitialisation()
        {
            _fixture.ClearAll();

            var testThing = new MockTestThing();
            _fixture.Ioc.RegisterSingleton<ITestThing>(testThing);

            var bundle = new MvxBundle();

            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();
            var toTest = new MvxDefaultViewModelLocator(navigationService);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            Assert.Throws<MvxException>(() =>
            {
                IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null, args);
            });
        }
    }
}
