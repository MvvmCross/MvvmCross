// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Core;
using MvvmCross.Tests;

namespace MvvmCross.UnitTest.Platform
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxStringToTypeParserTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxStringToTypeParserTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();
            _fixture.SetInvariantCulture();
        }

        [Test]
        public async Task Test_AllTypesAreSupported()
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.TypeSupported(typeof(string))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(int))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(long))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(short))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(float))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(uint))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(ulong))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(ushort))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(double))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(bool))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(Guid))).IsTrue();
            await Assert.That(parser.TypeSupported(typeof(StringSplitOptions))).IsTrue();
        }

        [Test]
        [Arguments("Hello, World")]
        [Arguments("Foo Bar Baz")]
        [Arguments("Z͚̭͖͟A͖̘̪L̻̯̥̬ͅG̞̰̭͍͖ͅͅO̘!̜")]
        [Arguments("")]
        [Arguments(null)]
        public async Task Test_StringCanBeRead(string testData)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(string), "ignored")).IsEqualTo(testData);
        }

        [Test]
        [Arguments("-123", -123.0)]
        [Arguments("1.23", 1.23)]
        [Arguments("1,23", 123.0)]
        [Arguments("1,230.00", 1230.0)]
        [Arguments("1.230,0", 0)]
        [Arguments("garbage", 0)]
        [Arguments("", 0)]
        [Arguments(null, 0)]
        public async Task Test_DoubleCanBeRead(string testData, double expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(double), "ignored")).IsEqualTo(expected);
        }

        [Test]
        [Arguments("-123", -123.0f)]
        [Arguments("1.23", 1.23f)]
        [Arguments("1,23", 123.0f)]
        [Arguments("1,230.00", 1230.0f)]
        [Arguments("1.230,0", 0f)]
        [Arguments("garbage", 0f)]
        [Arguments("", 0f)]
        [Arguments(null, 0f)]
        public async Task Test_FloatCanBeRead(string testData, float expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(float), "ignored")).IsEqualTo(expected);
        }

        [Test]
        [Arguments("-123", -123)]
        [Arguments("123", 123)]
        [Arguments("1.23", 0)]
        [Arguments("garbage", 0)]
        [Arguments("", 0)]
        [Arguments(null, 0)]
        public async Task Test_IntCanBeRead(string testData, int expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(int), "ignored")).IsEqualTo(expected);
        }

        [Test]
        [Arguments("-123", -123L)]
        [Arguments("123", 123L)]
        [Arguments("1.23", 0L)]
        [Arguments("garbage", 0L)]
        [Arguments("", 0L)]
        [Arguments(null, 0L)]
        public async Task Test_LongCanBeRead(string testData, long expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(long), "ignored")).IsEqualTo(expected);
        }

        [Test]
        [Arguments("123", 123ul)]
        [Arguments("1.23", 0ul)]
        [Arguments("garbage", 0ul)]
        [Arguments("", 0ul)]
        [Arguments(null, 0ul)]
        public async Task Test_ULongCanBeRead(string testData, ulong expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(ulong), "ignored")).IsEqualTo(expected);
        }

        [Test]
        [Arguments("123", (ushort)123)]
        [Arguments("1.23", (ushort)0)]
        [Arguments("garbage", (ushort)0)]
        [Arguments("", (ushort)0)]
        [Arguments(null, (ushort)0)]
        public async Task Test_UShortCanBeRead(string testData, ushort expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(ushort), "ignored")).IsEqualTo(expected);
        }

        [Test]
        [Arguments("true", true)]
        [Arguments("True", true)]
        [Arguments("tRue", true)]
        [Arguments("trUe", true)]
        [Arguments("truE", true)]
        [Arguments("TRUE", true)]
        [Arguments("false", false)]
        [Arguments("False", false)]
        [Arguments("fAlse", false)]
        [Arguments("faLse", false)]
        [Arguments("falSe", false)]
        [Arguments("falsE", false)]
        [Arguments("FALSE", false)]
        [Arguments("1.23", false)]
        [Arguments("garbage", false)]
        [Arguments("", false)]
        [Arguments(null, false)]
        public async Task Test_BoolCanBeRead(string testData, bool expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(bool), "ignored")).IsEqualTo(expected);
        }

        public static IEnumerable<(string, Guid)> GuidCases()
        {
            yield return ("{C3CF9078-0122-41BD-9E2D-D3199E937285}", Guid.Parse("{C3CF9078-0122-41BD-9E2D-D3199E937285}"));
            yield return ("{C3CF9078-0122-41BD-9E2D-D3199E937285}".ToLowerInvariant(), Guid.Parse("{C3CF9078-0122-41BD-9E2D-D3199E937285}"));
            yield return ("{8-0122-41BD-9E2D-D3199E937285}", Guid.Empty); // invalid
            yield return (Guid.Empty.ToString(), Guid.Empty);
            yield return ("", Guid.Empty);
            yield return ("garbage", Guid.Empty);
            yield return (null, Guid.Empty);
        }


        [Test]
        [MethodDataSource(nameof(GuidCases))]
        public async Task Test_GuidCanBeRead(string testData, Guid expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(Guid), "ignored")).IsEqualTo(expected);
        }

        [Test]
        [Arguments("RemoveEmptyEntries", StringSplitOptions.RemoveEmptyEntries)]
        [Arguments("None", StringSplitOptions.None)]
        [Arguments("none", StringSplitOptions.None)]
        [Arguments("garbage", StringSplitOptions.None)]
        [Arguments("", StringSplitOptions.None)]
        [Arguments(null, StringSplitOptions.None)]
        public async Task Test_EnumTypeCanBeRead(string testData, StringSplitOptions expected)
        {
            var parser = new MvxStringToTypeParser();
            await Assert.That(parser.ReadValue(testData, typeof(StringSplitOptions), "ignored")).IsEqualTo(expected);
        }
    }
}
