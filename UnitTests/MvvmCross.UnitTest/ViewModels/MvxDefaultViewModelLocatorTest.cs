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

namespace MvvmCross.UnitTest.ViewModels
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxDefaultViewModelLocatorTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxDefaultViewModelLocatorTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Test]
        public async Task Test_NoReloadState()
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
                TheString2 = string.Empty
            };
            var bundle = new MvxBundle();
            bundle.Write(testObject);

            var toTest = new MvxDefaultViewModelLocator();
            var args = new MvxNavigateEventArgs(NavigationMode.Show);

            IMvxViewModel viewModel = toTest.Load(typeof(Test1ViewModel), bundle, null, args);

            await Assert.That(viewModel).IsNotNull();
            var typedViewModel = (Test1ViewModel)viewModel;
            await Assert.That(typedViewModel.BundleInit).IsEqualTo(bundle);
            await Assert.That(typedViewModel.BundleState).IsNull();
            await Assert.That(typedViewModel.Thing).IsEqualTo(testThing);
            await Assert.That(typedViewModel.TheInitBundleSet).IsEqualTo(testObject);
            await Assert.That(typedViewModel.TheReloadBundleSet).IsNull();
            await Assert.That(typedViewModel.TheInitGuid1Set).IsEqualTo(testObject.TheGuid1);
            await Assert.That(typedViewModel.TheInitGuid2Set).IsEqualTo(testObject.TheGuid2);
            await Assert.That(typedViewModel.TheInitString1Set).IsEqualTo(testObject.TheString1);
            await Assert.That(typedViewModel.TheReloadGuid1Set).IsEqualTo(Guid.Empty);
            await Assert.That(typedViewModel.TheReloadGuid2Set).IsEqualTo(Guid.Empty);
            await Assert.That(typedViewModel.TheReloadString1Set).IsNull();
            await Assert.That(typedViewModel.StartCalled).IsTrue();
        }

        [Test]
        public async Task Test_WithReloadState()
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
                TheString2 = string.Empty
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
                TheString2 = string.Empty
            };
            var reloadBundle = new MvxBundle();
            reloadBundle.Write(reloadBundleObject);

            var toTest = new MvxDefaultViewModelLocator();
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            IMvxViewModel viewModel = toTest.Load(typeof(Test1ViewModel), initBundle, reloadBundle, args);

            await Assert.That(viewModel).IsNotNull();
            var typedViewModel = (Test1ViewModel)viewModel;
            await Assert.That(typedViewModel.BundleInit).IsEqualTo(initBundle);
            await Assert.That(typedViewModel.BundleState).IsEqualTo(reloadBundle);
            await Assert.That(typedViewModel.Thing).IsEqualTo(testThing);
            await Assert.That(typedViewModel.TheInitBundleSet).IsEqualTo(initBundleObject);
            await Assert.That(typedViewModel.TheReloadBundleSet).IsEqualTo(reloadBundleObject);
            await Assert.That(typedViewModel.TheInitGuid1Set).IsEqualTo(initBundleObject.TheGuid1);
            await Assert.That(typedViewModel.TheInitGuid2Set).IsEqualTo(initBundleObject.TheGuid2);
            await Assert.That(typedViewModel.TheInitString1Set).IsEqualTo(initBundleObject.TheString1);
            await Assert.That(typedViewModel.TheReloadGuid1Set).IsEqualTo(reloadBundleObject.TheGuid1);
            await Assert.That(typedViewModel.TheReloadGuid2Set).IsEqualTo(reloadBundleObject.TheGuid2);
            await Assert.That(typedViewModel.TheReloadString1Set).IsEqualTo(reloadBundleObject.TheString1);
            await Assert.That(typedViewModel.StartCalled).IsTrue();
        }

        [Test]
        public void Test_MissingDependency()
        {
            _fixture.ClearAll();

            var bundle = new MvxBundle();

            var toTest = new MvxDefaultViewModelLocator();
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            Assert.Throws<MvxException>(() =>
            {
                IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null, args);
            });
        }

        [Test]
        public void Test_FailingDependency()
        {
            _fixture.ClearAll();

            _fixture.Ioc.RegisterSingleton<ITestThing>(() => new FailingMockTestThing());

            var bundle = new MvxBundle();

            var toTest = new MvxDefaultViewModelLocator();
            var args = new MvxNavigateEventArgs(NavigationMode.Show);

            Assert.Throws<MvxException>(() =>
            {
                IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null, args);
            });
        }

        [Test]
        public void Test_FailingInitialisation()
        {
            _fixture.ClearAll();

            var testThing = new MockTestThing();
            _fixture.Ioc.RegisterSingleton<ITestThing>(testThing);

            var bundle = new MvxBundle();

            var toTest = new MvxDefaultViewModelLocator();
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            Assert.Throws<MvxException>(() =>
            {
                IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null, args);
            });
        }
    }
}
