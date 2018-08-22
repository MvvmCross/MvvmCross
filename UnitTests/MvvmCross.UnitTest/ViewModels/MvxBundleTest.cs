// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using MvvmCross.Core;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxBundleTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxBundleTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_RoundTrip()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

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

            var output = bundle.Read<BundleObject>();

            Assert.Equal(testObject, output);
        }

        public BundleObject TestFunction(string TheString1, string missing, int TheInt2, bool TheBool2, Guid TheGuid2)
        {
            return new BundleObject
            {
                TheString1 = TheString1,
                TheString2 = missing,
                TheInt2 = TheInt2,
                TheBool2 = TheBool2,
                TheGuid2 = TheGuid2
            };
        }

        [Fact]
        public void Test_FillArguments()
        {
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var testObject = new BundleObject
            {
                TheBool1 = false,
                TheBool2 = true,
                TheGuid1 = Guid.NewGuid(),
                TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                TheInt1 = 123,
                TheInt2 = 456,
                TheString1 = "Hello World",
                TheString2 = "Goo"
            };
            var bundle = new MvxBundle();
            bundle.Write(testObject);

            var method = GetType().GetMethod("TestFunction");
            var args = bundle.CreateArgumentList(method.GetParameters(), "ignored debug text");
            var output = method.Invoke(this, args.ToArray());

            var expected = new BundleObject
            {
                TheBool1 = false,
                TheBool2 = true,
                TheGuid1 = Guid.Empty,
                TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                TheInt1 = 0,
                TheInt2 = 456,
                TheString1 = "Hello World",
                TheString2 = null
            };
            Assert.Equal(expected, output);
        }
    }
}
