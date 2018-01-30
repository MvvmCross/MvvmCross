// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Test.Mocks.TestViewModels;
using Xunit;

namespace MvvmCross.Test.Platform
{
    [Collection("MvxTest")]
    public class MvxViewModelByNameLookupTest : IClassFixture<MvxTestFixture>
    {
        private readonly MvxTestFixture _fixture;

        public MvxViewModelByNameLookupTest(MvxTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_ViewModelByName_Finds_Expected_ViewModel()
        {
            _fixture.ClearAll();

            var assembly = GetType().Assembly;
            var finder = new MvxViewModelByNameLookup();
            finder.AddAll(assembly);
            Type result;
            Assert.True(finder.TryLookupByName("Test1ViewModel", out result));
            Assert.Equal(typeof(Test1ViewModel), result);
            Assert.True(finder.TryLookupByName("Test2ViewModel", out result));
            Assert.Equal(typeof(Test2ViewModel), result);
            Assert.True(finder.TryLookupByName("Test3ViewModel", out result));
            Assert.Equal(typeof(Test3ViewModel), result);
            Assert.False(finder.TryLookupByName("AbstractTest1ViewModel", out result));
            Assert.Null(result);
            Assert.False(finder.TryLookupByName("NoWayTestViewModel", out result));
            Assert.Null(result);
            Assert.True(finder.TryLookupByFullName("MvvmCross.Test.Mocks.TestViewModels.Test1ViewModel",
                                                     out result));
            Assert.Equal(typeof(Test1ViewModel), result);
            Assert.True(finder.TryLookupByFullName("MvvmCross.Test.Mocks.TestViewModels.Test2ViewModel",
                                                     out result));
            Assert.Equal(typeof(Test2ViewModel), result);
            Assert.True(finder.TryLookupByFullName("MvvmCross.Test.Mocks.TestViewModels.Test3ViewModel",
                                                     out result));
            Assert.Equal(typeof(Test3ViewModel), result);
            Assert.False(
                finder.TryLookupByFullName("MvvmCross.Test.Mocks.TestViewModels.AbstractTest1ViewModel",
                                           out result));
            Assert.Null(result);
            Assert.False(finder.TryLookupByFullName(
                "MvvmCross.Test.Mocks.TestViewModels.NoWayTestViewModel", out result));
            Assert.Null(result);
        }
    }
}
