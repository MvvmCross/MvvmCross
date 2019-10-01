// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.UnitTest.Mocks.TestViews;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.Platform
{
    [Collection("MvxTest")]
    public class MvxViewModelViewLookupBuilderTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxViewModelViewLookupBuilderTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_Builder()
        {
            _fixture.ClearAll();

            var assembly = GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(assembly);
            var nameMapping = new MvxPostfixAwareViewToViewModelNameMapping("View", "Oddness");
            var finder = new MvxViewModelViewTypeFinder(viewModelNameLookup, nameMapping);
            _fixture.Ioc.RegisterSingleton<IMvxViewModelTypeFinder>(finder);

            var builder = new MvxViewModelViewLookupBuilder();
            var result = builder.Build(new[] { assembly });

            Assert.Equal(4, result.Count);
            Assert.Equal(typeof(Test1View), result[typeof(Test1ViewModel)]);
            Assert.Equal(typeof(NotTest2View), result[typeof(Test2ViewModel)]);
            Assert.Equal(typeof(NotTest3View), result[typeof(Test3ViewModel)]);
            Assert.Equal(typeof(OddNameOddness), result[typeof(OddNameViewModel)]);
        }
    }
}
