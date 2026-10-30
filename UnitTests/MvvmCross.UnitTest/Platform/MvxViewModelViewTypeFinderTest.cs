// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.UnitTest.Mocks.TestViews;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.UnitTest.Platform
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxViewModelViewTypeFinderTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxViewModelViewTypeFinderTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Test]
        public async Task Test_MvxViewModelViewTypeFinder()
        {
            _fixture.ClearAll();

            var assembly = GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(assembly);
            var nameMapping = new MvxPostfixAwareViewToViewModelNameMapping("View", "Oddness");
            var test = new MvxViewModelViewTypeFinder(viewModelNameLookup, nameMapping);

            // test for positives
            var result = test.FindTypeOrNull(typeof(Test1View));
            await Assert.That(result).IsEqualTo(typeof(Test1ViewModel));
            result = test.FindTypeOrNull(typeof(NotTest2View));
            await Assert.That(result).IsEqualTo(typeof(Test2ViewModel));
            result = test.FindTypeOrNull(typeof(NotTest3View));
            await Assert.That(result).IsEqualTo(typeof(Test3ViewModel));
            result = test.FindTypeOrNull(typeof(OddNameOddness));
            await Assert.That(result).IsEqualTo(typeof(OddNameViewModel));

            // test for negatives
            result = test.FindTypeOrNull(typeof(AbstractTest1View));
            await Assert.That(result).IsNull();
            result = test.FindTypeOrNull(typeof(NotReallyAView));
            await Assert.That(result).IsNull();
        }

        [Test]
        public async Task Test_FindTypeOrNull_WithRuntimeType_FindsConcreteViewType()
        {
            _fixture.ClearAll();

            var assembly = GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(assembly);
            var nameMapping = new MvxPostfixAwareViewToViewModelNameMapping("View", "Oddness");
            var test = new MvxViewModelViewTypeFinder(viewModelNameLookup, nameMapping);

            // Simulate the scenario where a view is accessed through a base interface
            // but we need to find the ViewModel based on the concrete type
            IMvxView view1 = new Test1View();
            var result = test.FindTypeOrNull(view1.GetType());
            await Assert.That(result).IsEqualTo(typeof(Test1ViewModel));

            IMvxView view2 = new NotTest2View();
            result = test.FindTypeOrNull(view2.GetType());
            await Assert.That(result).IsEqualTo(typeof(Test2ViewModel));

            IMvxView view3 = new NotTest3View();
            result = test.FindTypeOrNull(view3.GetType());
            await Assert.That(result).IsEqualTo(typeof(Test3ViewModel));

            IMvxView view4 = new OddNameOddness();
            result = test.FindTypeOrNull(view4.GetType());
            await Assert.That(result).IsEqualTo(typeof(OddNameViewModel));
        }

        [Test]
        public async Task Test_FindTypeOrNull_WithInterfaceType_FailsToFindViewModel()
        {
            _fixture.ClearAll();

            var assembly = GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(assembly);
            var nameMapping = new MvxPostfixAwareViewToViewModelNameMapping("View", "Oddness");
            var test = new MvxViewModelViewTypeFinder(viewModelNameLookup, nameMapping);

            // This test demonstrates the bug: if we pass the interface type instead of
            // the concrete type, we cannot find the associated ViewModel
            var result = test.FindTypeOrNull(typeof(IMvxView));
            await Assert.That(result).IsNull();
        }

        [Test]
        public async Task Test_FindAssociatedViewModelTypeOrNull_UsesConcreteType()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxViewModelTypeFinder>(CreateViewModelTypeFinder());

            // Test that the extension method correctly uses the concrete type,
            // not the compile-time generic type
            IMvxView view1 = new Test1View();
            var result = view1.FindAssociatedViewModelTypeOrNull();
            await Assert.That(result).IsEqualTo(typeof(Test1ViewModel));

            IMvxView view2 = new NotTest2View();
            result = view2.FindAssociatedViewModelTypeOrNull();
            await Assert.That(result).IsEqualTo(typeof(Test2ViewModel));

            IMvxView view3 = new NotTest3View();
            result = view3.FindAssociatedViewModelTypeOrNull();
            await Assert.That(result).IsEqualTo(typeof(Test3ViewModel));
        }

        private IMvxViewModelTypeFinder CreateViewModelTypeFinder()
        {
            var assembly = GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(assembly);
            var nameMapping = new MvxPostfixAwareViewToViewModelNameMapping("View", "Oddness");
            return new MvxViewModelViewTypeFinder(viewModelNameLookup, nameMapping);
        }
    }
}
