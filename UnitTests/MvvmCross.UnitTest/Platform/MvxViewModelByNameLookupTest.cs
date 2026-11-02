// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Platform
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxViewModelByNameLookupTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxViewModelByNameLookupTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Test]
        public async Task Test_ViewModelByName_Finds_Expected_ViewModel()
        {
            _fixture.ClearAll();

            var assembly = GetType().Assembly;
            var finder = new MvxViewModelByNameLookup();
            finder.AddAll(assembly);
            Type result;
            await Assert.That(finder.TryLookupByName("Test1ViewModel", out result)).IsTrue();
            await Assert.That(result).IsEqualTo(typeof(Test1ViewModel));
            await Assert.That(finder.TryLookupByName("Test2ViewModel", out result)).IsTrue();
            await Assert.That(result).IsEqualTo(typeof(Test2ViewModel));
            await Assert.That(finder.TryLookupByName("Test3ViewModel", out result)).IsTrue();
            await Assert.That(result).IsEqualTo(typeof(Test3ViewModel));
            await Assert.That(finder.TryLookupByName("AbstractTest1ViewModel", out result)).IsFalse();
            await Assert.That(result).IsNull();
            await Assert.That(finder.TryLookupByName("NoWayTestViewModel", out result)).IsFalse();
            await Assert.That(result).IsNull();
            await Assert.That(finder.TryLookupByFullName("MvvmCross.UnitTest.Mocks.TestViewModels.Test1ViewModel",
                                                     out result)).IsTrue();
            await Assert.That(result).IsEqualTo(typeof(Test1ViewModel));
            await Assert.That(finder.TryLookupByFullName("MvvmCross.UnitTest.Mocks.TestViewModels.Test2ViewModel",
                                                     out result)).IsTrue();
            await Assert.That(result).IsEqualTo(typeof(Test2ViewModel));
            await Assert.That(finder.TryLookupByFullName("MvvmCross.UnitTest.Mocks.TestViewModels.Test3ViewModel",
                                                     out result)).IsTrue();
            await Assert.That(result).IsEqualTo(typeof(Test3ViewModel));
            await Assert.That(finder.TryLookupByFullName("MvvmCross.UnitTest.Mocks.TestViewModels.AbstractTest1ViewModel",
                                           out result)).IsFalse();
            await Assert.That(result).IsNull();
            await Assert.That(finder.TryLookupByFullName(
                "MvvmCross.UnitTest.Mocks.TestViewModels.NoWayTestViewModel", out result)).IsFalse();
            await Assert.That(result).IsNull();
        }
    }
}
