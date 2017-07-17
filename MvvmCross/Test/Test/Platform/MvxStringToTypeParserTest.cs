// MvxStringToTypeParserTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.Platform;
using MvvmCross.Test.Core;
using NUnit.Framework;

namespace MvvmCross.Test.Platform
{
    [TestFixture]
    public class MvxStringToTypeParserTest : MvxIoCSupportingTest
    {
        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            SetInvariantCulture();
        }

        [Test]
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

        [Test]
        [TestCase("Hello, World")]
        [TestCase("Foo Bar Baz")]
        [TestCase("Z͚̭͖͟A͖̘̪L̻̯̥̬ͅG̞̰̭͍͖ͅͅO̘!̜")]
        [TestCase("")]
        [TestCase(null)]
        public void Test_StringCanBeRead(string testData)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(testData, parser.ReadValue(testData, typeof(string), "ignored"));
        }

        [Test]
        [TestCase("-123", -123.0)]
        [TestCase("1.23", 1.23)]
        [TestCase("1,23", 123.0)]
        [TestCase("1,230.00", 1230.0)]
        [TestCase("1.230,0", 0)]
        [TestCase("garbage", 0)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        public void Test_DoubleCanBeRead(string testData, double expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(double), "ignored"));
        }

        [Test]
        [TestCase("-123", -123.0f)]
        [TestCase("1.23", 1.23f)]
        [TestCase("1,23", 123.0f)]
        [TestCase("1,230.00", 1230.0f)]
        [TestCase("1.230,0", 0f)]
        [TestCase("garbage", 0f)]
        [TestCase("", 0f)]
        [TestCase(null, 0f)]
        public void Test_FloatCanBeRead(string testData, float expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(float), "ignored"));
        }

        [Test]
        [TestCase("-123", -123)]
        [TestCase("123", 123)]
        [TestCase("1.23", 0)]
        [TestCase("garbage", 0)]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        public void Test_IntCanBeRead(string testData, int expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(int), "ignored"));
        }

        [Test]
        [TestCase("-123", -123L)]
        [TestCase("123", 123L)]
        [TestCase("1.23", 0L)]
        [TestCase("garbage", 0L)]
        [TestCase("", 0L)]
        [TestCase(null, 0L)]
        public void Test_LongCanBeRead(string testData, long expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(long), "ignored"));
        }

        [Test]
        [TestCase("123", 123ul)]
        [TestCase("1.23", 0ul)]
        [TestCase("garbage", 0ul)]
        [TestCase("", 0ul)]
        [TestCase(null, 0ul)]
        public void Test_ULongCanBeRead(string testData, ulong expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(ulong), "ignored"));
        }

        [Test]
        [TestCase("123", (ushort)123)]
        [TestCase("1.23", (ushort)0)]
        [TestCase("garbage", (ushort)0)]
        [TestCase("", (ushort)0)]
        [TestCase(null, (ushort)0)]
        public void Test_UShortCanBeRead(string testData, ushort expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(ushort), "ignored"));
        }

        [Test]
        [TestCase("true", true)]
        [TestCase("True", true)]
        [TestCase("tRue", true)]
        [TestCase("trUe", true)]
        [TestCase("truE", true)]
        [TestCase("TRUE", true)]
        [TestCase("false", false)]
        [TestCase("False", false)]
        [TestCase("fAlse", false)]
        [TestCase("faLse", false)]
        [TestCase("falSe", false)]
        [TestCase("falsE", false)]
        [TestCase("FALSE", false)]
        [TestCase("1.23", false)]
        [TestCase("garbage", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void Test_BoolCanBeRead(string testData, bool expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(bool), "ignored"));
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
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(Guid), "ignored"));
        }

        [Test]
        [TestCase("RemoveEmptyEntries", StringSplitOptions.RemoveEmptyEntries)]
        [TestCase("None", StringSplitOptions.None)]
        [TestCase("none", StringSplitOptions.None)]
        [TestCase("garbage", StringSplitOptions.None)]
        [TestCase("", StringSplitOptions.None)]
        [TestCase(null, StringSplitOptions.None)]
        public void Test_EnumTypeCanBeRead(string testData, StringSplitOptions expected)
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual(expected, parser.ReadValue(testData, typeof(StringSplitOptions), "ignored"));
        }
    }
}
