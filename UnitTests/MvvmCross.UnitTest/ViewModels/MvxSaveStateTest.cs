// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Core;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxSaveStateTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxSaveStateTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_SaveState()
        {
            _fixture.ClearAll();

            _fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var viewModel = new Test3ViewModel
            {
                AdditionalSaveStateFields = new Dictionary<string, string>
                {
                    { "Life1", "John" },
                    { "Life2", "Jane" }
                },
                SaveStateBundleObject = new BundleObject
                {
                    TheBool1 = false,
                    TheBool2 = true,
                    TheGuid1 = Guid.NewGuid(),
                    TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                    TheInt1 = 123,
                    TheInt2 = 456,
                    TheString1 = "Hello World",
                    TheString2 = null
                }
            };

            var bundle = viewModel.SaveStateBundle();

            var extracted = bundle.Read<BundleObject>();
            Assert.Equal(viewModel.SaveStateBundleObject, extracted);
            Assert.Equal("John", bundle.Data["Life1"]);
            Assert.Equal("Jane", bundle.Data["Life2"]);
        }

        [Fact]
        public void Test_NullSaveState()
        {
            _fixture.ClearAll();

            var viewModel = new Test3ViewModel();

            var bundle = viewModel.SaveStateBundle();
            Assert.Equal(0, bundle.Data.Count);
        }
    }
}
