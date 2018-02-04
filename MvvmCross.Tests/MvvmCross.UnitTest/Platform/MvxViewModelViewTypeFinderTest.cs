﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.ViewModels;
using MvvmCross.Test.Mocks.TestViewModels;
using MvvmCross.Test.Mocks.TestViews;
using Xunit;

namespace MvvmCross.Test.Platform
{
    [Collection("MvxTest")]
    public class MvxViewModelViewTypeFinderTest : IClassFixture<MvxTestFixture>
    {
        private readonly MvxTestFixture _fixture;

        public MvxViewModelViewTypeFinderTest(MvxTestFixture fixture)
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
    }
}
