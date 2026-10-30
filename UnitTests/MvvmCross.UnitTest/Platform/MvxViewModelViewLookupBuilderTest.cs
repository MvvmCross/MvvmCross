// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.UnitTest.Mocks.TestViews;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Platform
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxViewModelViewLookupBuilderTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxViewModelViewLookupBuilderTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Test]
        public async Task Test_Builder()
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

            await Assert.That(result.Count).IsEqualTo(4);
            await Assert.That(result[typeof(Test1ViewModel)]).IsEqualTo(typeof(Test1View));
            await Assert.That(result[typeof(Test2ViewModel)]).IsEqualTo(typeof(NotTest2View));
            await Assert.That(result[typeof(Test3ViewModel)]).IsEqualTo(typeof(NotTest3View));
            await Assert.That(result[typeof(OddNameViewModel)]).IsEqualTo(typeof(OddNameOddness));
        }
    }
}
