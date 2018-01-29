// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core.Platform;
using MvvmCross.Test.Core;
using Xunit;

namespace MvvmCross.Test.Platform
{
    
    public class MvxStringToTypeParserTest : MvxIoCSupportingTest
    {
        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            SetInvariantCulture();
        }

        [Fact]
        public void Test_AllTypesAreSupported()
        {
            var parser = new MvxStringToTypeParser();
            Assert.IsTrue(parser.TypeSupported(typeof(string)));
            Assert.IsTrue(parser.TypeSupported(typeof(int)));
            Assert.IsTrue(parser.TypeSupported(typeof(long)));
            Assert.IsTrue(parser.TypeSupported(typeof(short)));
            Assert.IsTrue(parser.TypeSupported(typeof(float)));
            Assert.IsTrue(parser.TypeSupported(typeof(uint)));
            Assert.IsTrue(parser.TypeSupported(typeof(ulong)));
            Assert.IsTrue(parser.TypeSupported(typeof(ushort)));
            Assert.IsTrue(parser.TypeSupported(typeof(double)));
            Assert.IsTrue(parser.TypeSupported(typeof(bool)));
            Assert.IsTrue(parser.TypeSupported(typeof(Guid)));
            Assert.IsTrue(parser.TypeSupported(typeof(StringSplitOptions)));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        private static object[] _guidCases =
        {
            new object[] { "{C3CF9078-0122-41BD-9E2D-D3199E937285}", Guid.Parse("{C3CF9078-0122-41BD-9E2D-D3199E937285}") },
            new object[] { "{C3CF9078-0122-41BD-9E2D-D3199E937285}".ToLowerInvariant(), Guid.Parse("{C3CF9078-0122-41BD-9E2D-D3199E937285}") },
            new object[] { "{8-0122-41BD-9E2D-D3199E937285}", Guid.Empty }, // invalid
            new object[] { Guid.Empty.ToString(), Guid.Empty },
            new object[] { "", Guid.Empty },
            new object[] { "garbage", Guid.Empty },
            new object[] { null, Guid.Empty }
        };

        [Test, TestCaseSource(nameof(_guidCases))]
        public void Test_GuidCanBeRead(string testData, Guid expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.Equal(expected, parser.ReadValue(testData, typeof(Guid), "ignored"));
        }

        [Fact]
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
