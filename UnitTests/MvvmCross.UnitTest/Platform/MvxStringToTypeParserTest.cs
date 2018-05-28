// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Core;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.UnitTest.Platform
{
    [Collection("MvxTest")]
    public class MvxStringToTypeParserTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxStringToTypeParserTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();
            _fixture.SetInvariantCulture();
        }

        [Fact]
        public void Test_AllTypesAreSupported()
        {
            var parser = new MvxStringToTypeParser();
            Assert.True(parser.TypeSupported(typeof(string)));
            Assert.True(parser.TypeSupported(typeof(int)));
            Assert.True(parser.TypeSupported(typeof(long)));
            Assert.True(parser.TypeSupported(typeof(short)));
            Assert.True(parser.TypeSupported(typeof(float)));
            Assert.True(parser.TypeSupported(typeof(uint)));
            Assert.True(parser.TypeSupported(typeof(ulong)));
            Assert.True(parser.TypeSupported(typeof(ushort)));
            Assert.True(parser.TypeSupported(typeof(double)));
            Assert.True(parser.TypeSupported(typeof(bool)));
            Assert.True(parser.TypeSupported(typeof(Guid)));
            Assert.True(parser.TypeSupported(typeof(StringSplitOptions)));
        }

        [Theory]
        [InlineData("Hello, World")]
        [InlineData("Foo Bar Baz")]
        [InlineData("Z͚̭͖͟A͖̘̪L̻̯̥̬ͅG̞̰̭͍͖ͅͅO̘!̜")]
        [InlineData("")]
        [InlineData(null)]
        public void Test_StringCanBeRead(string testData)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(testData, parser.ReadValue(testData, typeof(string), "ignored"));
        }

        [Theory]
        [InlineData("-123", -123.0)]
        [InlineData("1.23", 1.23)]
        [InlineData("1,23", 123.0)]
        [InlineData("1,230.00", 1230.0)]
        [InlineData("1.230,0", 0)]
        [InlineData("garbage", 0)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void Test_DoubleCanBeRead(string testData, double expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(double), "ignored"));
        }

        [Theory]
        [InlineData("-123", -123.0f)]
        [InlineData("1.23", 1.23f)]
        [InlineData("1,23", 123.0f)]
        [InlineData("1,230.00", 1230.0f)]
        [InlineData("1.230,0", 0f)]
        [InlineData("garbage", 0f)]
        [InlineData("", 0f)]
        [InlineData(null, 0f)]
        public void Test_FloatCanBeRead(string testData, float expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(float), "ignored"));
        }

        [Theory]
        [InlineData("-123", -123)]
        [InlineData("123", 123)]
        [InlineData("1.23", 0)]
        [InlineData("garbage", 0)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void Test_IntCanBeRead(string testData, int expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(int), "ignored"));
        }

        [Theory]
        [InlineData("-123", -123L)]
        [InlineData("123", 123L)]
        [InlineData("1.23", 0L)]
        [InlineData("garbage", 0L)]
        [InlineData("", 0L)]
        [InlineData(null, 0L)]
        public void Test_LongCanBeRead(string testData, long expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(long), "ignored"));
        }

        [Theory]
        [InlineData("123", 123ul)]
        [InlineData("1.23", 0ul)]
        [InlineData("garbage", 0ul)]
        [InlineData("", 0ul)]
        [InlineData(null, 0ul)]
        public void Test_ULongCanBeRead(string testData, ulong expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(ulong), "ignored"));
        }

        [Theory]
        [InlineData("123", (ushort)123)]
        [InlineData("1.23", (ushort)0)]
        [InlineData("garbage", (ushort)0)]
        [InlineData("", (ushort)0)]
        [InlineData(null, (ushort)0)]
        public void Test_UShortCanBeRead(string testData, ushort expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(ushort), "ignored"));
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("tRue", true)]
        [InlineData("trUe", true)]
        [InlineData("truE", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("fAlse", false)]
        [InlineData("faLse", false)]
        [InlineData("falSe", false)]
        [InlineData("falsE", false)]
        [InlineData("FALSE", false)]
        [InlineData("1.23", false)]
        [InlineData("garbage", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void Test_BoolCanBeRead(string testData, bool expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(bool), "ignored"));
        }

        public static IEnumerable<object[]> GuidCases()
        {
            yield return new object[] { "{C3CF9078-0122-41BD-9E2D-D3199E937285}", Guid.Parse("{C3CF9078-0122-41BD-9E2D-D3199E937285}") };
            yield return new object[] { "{C3CF9078-0122-41BD-9E2D-D3199E937285}".ToLowerInvariant(), Guid.Parse("{C3CF9078-0122-41BD-9E2D-D3199E937285}") };
            yield return new object[] { "{8-0122-41BD-9E2D-D3199E937285}", Guid.Empty }; // invalid
            yield return new object[] { Guid.Empty.ToString(), Guid.Empty };
            yield return new object[] { "", Guid.Empty };
            yield return new object[] { "garbage", Guid.Empty };
            yield return new object[] { null, Guid.Empty };
        }
        

        [Theory]
        [MemberData(nameof(GuidCases))]
        public void Test_GuidCanBeRead(string testData, Guid expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(Guid), "ignored"));
        }

        [Theory]
        [InlineData("RemoveEmptyEntries", StringSplitOptions.RemoveEmptyEntries)]
        [InlineData("None", StringSplitOptions.None)]
        [InlineData("none", StringSplitOptions.None)]
        [InlineData("garbage", StringSplitOptions.None)]
        [InlineData("", StringSplitOptions.None)]
        [InlineData(null, StringSplitOptions.None)]
        public void Test_EnumTypeCanBeRead(string testData, StringSplitOptions expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(StringSplitOptions), "ignored"));
        }
    }
}
