// MvxDefaultViewModelLocatorTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.ViewModels
{
    using System;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Test.Mocks.TestViewModels;

    using NUnit.Framework;

    [TestFixture]
    public class MvxDefaultViewModelLocatorTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_NoReloadState()
        {
            ClearAll();

            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var testThing = new MockTestThing();
            Ioc.RegisterSingleton<ITestThing>(testThing);

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

            var toTest = new MvxDefaultViewModelLocator();

            IMvxViewModel viewModel = toTest.Load(typeof(Test1ViewModel), bundle, null);

            Assert.IsNotNull(viewModel);
            var typedViewModel = (Test1ViewModel)viewModel;
            Assert.AreSame(bundle, typedViewModel.BundleInit);
            Assert.IsNull(typedViewModel.BundleState);
            Assert.AreSame(testThing, typedViewModel.Thing);
            Assert.AreEqual(testObject, typedViewModel.TheInitBundleSet);
            Assert.IsNull(typedViewModel.TheReloadBundleSet);
            Assert.AreEqual(testObject.TheGuid1, typedViewModel.TheInitGuid1Set);
            Assert.AreEqual(testObject.TheGuid2, typedViewModel.TheInitGuid2Set);
            Assert.AreEqual(testObject.TheString1, typedViewModel.TheInitString1Set);
            Assert.AreEqual(Guid.Empty, typedViewModel.TheReloadGuid1Set);
            Assert.AreEqual(Guid.Empty, typedViewModel.TheReloadGuid2Set);
            Assert.AreEqual(null, typedViewModel.TheReloadString1Set);
            Assert.IsTrue(typedViewModel.StartCalled);
        }

        [Test]
        public void Test_WithReloadState()
        {
            ClearAll();

            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var testThing = new MockTestThing();
            Ioc.RegisterSingleton<ITestThing>(testThing);

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

            var toTest = new MvxDefaultViewModelLocator();
            IMvxViewModel viewModel = toTest.Load(typeof(Test1ViewModel), initBundle, reloadBundle);

            Assert.IsNotNull(viewModel);
            var typedViewModel = (Test1ViewModel)viewModel;
            Assert.AreSame(initBundle, typedViewModel.BundleInit);
            Assert.AreSame(reloadBundle, typedViewModel.BundleState);
            Assert.AreSame(testThing, typedViewModel.Thing);
            Assert.AreEqual(initBundleObject, typedViewModel.TheInitBundleSet);
            Assert.AreEqual(reloadBundleObject, typedViewModel.TheReloadBundleSet);
            Assert.AreEqual(initBundleObject.TheGuid1, typedViewModel.TheInitGuid1Set);
            Assert.AreEqual(initBundleObject.TheGuid2, typedViewModel.TheInitGuid2Set);
            Assert.AreEqual(initBundleObject.TheString1, typedViewModel.TheInitString1Set);
            Assert.AreEqual(reloadBundleObject.TheGuid1, typedViewModel.TheReloadGuid1Set);
            Assert.AreEqual(reloadBundleObject.TheGuid2, typedViewModel.TheReloadGuid2Set);
            Assert.AreEqual(reloadBundleObject.TheString1, typedViewModel.TheReloadString1Set);
            Assert.IsTrue(typedViewModel.StartCalled);
        }

        [Test]
        public void Test_MissingDependency()
        {
            ClearAll();

            var bundle = new MvxBundle();

            var toTest = new MvxDefaultViewModelLocator();

            Assert.That(
                () => {
                    IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null);
                },
                Throws.TypeOf<MvxException>().With.Message.StartWith("Problem creating viewModel"));
        }

        [Test]
        public void Test_FailingDependency()
        {
            ClearAll();

            Ioc.RegisterSingleton<ITestThing>(() => new FailingMockTestThing());

            var bundle = new MvxBundle();

            var toTest = new MvxDefaultViewModelLocator();

            Assert.That(
                () => {
                    IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null);
                },
                Throws.TypeOf<MvxException>().With.Message.StartWith("Problem creating viewModel"));
        }

        [Test]
        public void Test_FailingInitialisation()
        {
            ClearAll();

            var testThing = new MockTestThing();
            Ioc.RegisterSingleton<ITestThing>(testThing);

            var bundle = new MvxBundle();

            var toTest = new MvxDefaultViewModelLocator();

            Assert.That(
                () => {
                    IMvxViewModel viewModel = toTest.Load(typeof(Test4ViewModel), bundle, null);
                },
                Throws.TypeOf<MvxException>().With.Message.StartWith("Problem initialising viewModel"));
        }
    }
}