// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.UnitTest.Mocks.TestViews;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Xunit;

namespace MvvmCross.UnitTest.Platform
{
    [Collection("MvxTest")]
    public class MvxViewModelViewTypeFinderTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxViewModelViewTypeFinderTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_MvxViewModelViewTypeFinder()
        {
            _fixture.ClearAll();

            var assembly = GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(assembly);
            var nameMapping = new MvxPostfixAwareViewToViewModelNameMapping("View", "Oddness");
            var test = new MvxViewModelViewTypeFinder(viewModelNameLookup, nameMapping);

            // test for positives
            var result = test.FindTypeOrNull(typeof(Test1View));
            Assert.Equal(typeof(Test1ViewModel), result);
            result = test.FindTypeOrNull(typeof(NotTest2View));
            Assert.Equal(typeof(Test2ViewModel), result);
            result = test.FindTypeOrNull(typeof(NotTest3View));
            Assert.Equal(typeof(Test3ViewModel), result);
            result = test.FindTypeOrNull(typeof(OddNameOddness));
            Assert.Equal(typeof(OddNameViewModel), result);

            // test for negatives
            result = test.FindTypeOrNull(typeof(AbstractTest1View));
            Assert.Null(result);
            result = test.FindTypeOrNull(typeof(NotReallyAView));
            Assert.Null(result);
        }

        [Fact]
        public void Test_FindTypeOrNull_WithRuntimeType_FindsConcreteViewType()
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
            Assert.Equal(typeof(Test1ViewModel), result);

            IMvxView view2 = new NotTest2View();
            result = test.FindTypeOrNull(view2.GetType());
            Assert.Equal(typeof(Test2ViewModel), result);

            IMvxView view3 = new NotTest3View();
            result = test.FindTypeOrNull(view3.GetType());
            Assert.Equal(typeof(Test3ViewModel), result);

            IMvxView view4 = new OddNameOddness();
            result = test.FindTypeOrNull(view4.GetType());
            Assert.Equal(typeof(OddNameViewModel), result);
        }

        [Fact]
        public void Test_FindTypeOrNull_WithInterfaceType_FailsToFindViewModel()
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
            Assert.Null(result);
        }

        [Fact]
        public void Test_FindAssociatedViewModelTypeOrNull_UsesConcreteType()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxViewModelTypeFinder>(CreateViewModelTypeFinder());

            // Test that the extension method correctly uses the concrete type,
            // not the compile-time generic type
            IMvxView view1 = new Test1View();
            var result = view1.FindAssociatedViewModelTypeOrNull();
            Assert.Equal(typeof(Test1ViewModel), result);

            IMvxView view2 = new NotTest2View();
            result = view2.FindAssociatedViewModelTypeOrNull();
            Assert.Equal(typeof(Test2ViewModel), result);

            IMvxView view3 = new NotTest3View();
            result = view3.FindAssociatedViewModelTypeOrNull();
            Assert.Equal(typeof(Test3ViewModel), result);
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
