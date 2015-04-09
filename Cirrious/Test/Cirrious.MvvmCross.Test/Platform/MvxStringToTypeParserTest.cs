// MvxStringToTypeParserTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Test.Core;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test.Platform
{
    [TestFixture]
    public class MvxStringToTypeParserTest : MvxIoCSupportingTest
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            SetInvariantCulture();
        }

        [Test]
        public void Test_AllTypesAreSupported()
        {
            var parser = new MvxStringToTypeParser();
            Assert.IsTrue(parser.TypeSupported(typeof (string)));
            Assert.IsTrue(parser.TypeSupported(typeof (int)));
            Assert.IsTrue(parser.TypeSupported(typeof (long)));
            Assert.IsTrue(parser.TypeSupported(typeof (short)));
            Assert.IsTrue(parser.TypeSupported(typeof (float)));
            Assert.IsTrue(parser.TypeSupported(typeof (uint)));
            Assert.IsTrue(parser.TypeSupported(typeof (ulong)));
            Assert.IsTrue(parser.TypeSupported(typeof (ushort)));
            Assert.IsTrue(parser.TypeSupported(typeof (double)));
            Assert.IsTrue(parser.TypeSupported(typeof (bool)));
            Assert.IsTrue(parser.TypeSupported(typeof (Guid)));
            Assert.IsTrue(parser.TypeSupported(typeof (StringSplitOptions)));
        }

        [Test]
        public void Test_AllTypesCanBeRead()
        {
            var parser = new MvxStringToTypeParser();
            Assert.AreEqual("Hello World", parser.ReadValue("Hello World", typeof (string), "ignored"));
            Assert.AreEqual("", parser.ReadValue("", typeof (string), "ignored"));
            Assert.AreEqual(null, parser.ReadValue(null, typeof (string), "ignored"));

            Assert.AreEqual(1.23, parser.ReadValue("1.23", typeof (double), "ignored"));
            Assert.AreEqual(123.0, parser.ReadValue("1,23", typeof (double), "ignored"),
                            "comma separators ignored under invariant parsing");
            Assert.AreEqual(0, parser.ReadValue("garbage", typeof (double), "ignored"));
            Assert.AreEqual(0, parser.ReadValue("", typeof (double), "ignored"));
            Assert.AreEqual(0, parser.ReadValue(null, typeof (double), "ignored"));

            Assert.AreEqual(1.23f, parser.ReadValue("1.23", typeof (float), "ignored"));
            Assert.AreEqual(123.0f, parser.ReadValue("1,23", typeof (float), "ignored"),
                            "comma separators ignored under invariant parsing");
            Assert.AreEqual(0f, parser.ReadValue("garbage", typeof (float), "ignored"));
            Assert.AreEqual(0f, parser.ReadValue("", typeof (float), "ignored"));
            Assert.AreEqual(0f, parser.ReadValue(null, typeof (float), "ignored"));

            Assert.AreEqual(123, parser.ReadValue("123", typeof (int), "ignored"));
            Assert.AreEqual(0, parser.ReadValue("12.3", typeof (int), "ignored"),
                            "partial integers should not be parsed");
            Assert.AreEqual(0, parser.ReadValue("garbage", typeof (int), "ignored"));
            Assert.AreEqual(0, parser.ReadValue("", typeof (int), "ignored"));
            Assert.AreEqual(0, parser.ReadValue(null, typeof (int), "ignored"));

            Assert.AreEqual(123L, parser.ReadValue("123", typeof (long), "ignored"));
            Assert.AreEqual(0, parser.ReadValue("12.3", typeof (long), "ignored"),
                            "partial integers should not be parsed");
            Assert.AreEqual(0L, parser.ReadValue("garbage", typeof (long), "ignored"));
            Assert.AreEqual(0L, parser.ReadValue("", typeof (long), "ignored"));
            Assert.AreEqual(0L, parser.ReadValue(null, typeof (long), "ignored"));

            Assert.AreEqual((ulong) 123L, parser.ReadValue("123", typeof (ulong), "ignored"));
            Assert.AreEqual((ulong) 0, parser.ReadValue("12.3", typeof (ulong), "ignored"),
                            "partial integers should not be parsed");
            Assert.AreEqual((ulong) 0L, parser.ReadValue("garbage", typeof (ulong), "ignored"));
            Assert.AreEqual((ulong) 0L, parser.ReadValue("", typeof (ulong), "ignored"));
            Assert.AreEqual((ulong) 0L, parser.ReadValue(null, typeof (ulong), "ignored"));

            Assert.AreEqual((ushort) 123, parser.ReadValue("123", typeof (ushort), "ignored"));
            Assert.AreEqual((ushort) 0, parser.ReadValue("12.3", typeof (ushort), "ignored"),
                            "partial integers should not be parsed");
            Assert.AreEqual((ushort) 0, parser.ReadValue("garbage", typeof (ushort), "ignored"));
            Assert.AreEqual((ushort) 0, parser.ReadValue("", typeof (ushort), "ignored"));
            Assert.AreEqual((ushort) 0, parser.ReadValue(null, typeof (ushort), "ignored"));

            Assert.AreEqual(true, parser.ReadValue("true", typeof (bool), "ignored"));
            Assert.AreEqual(true, parser.ReadValue("True", typeof (bool), "ignored"));
            Assert.AreEqual(true, parser.ReadValue("TRUE", typeof (bool), "ignored"));
            Assert.AreEqual(true, parser.ReadValue("truE", typeof (bool), "ignored"));
            Assert.AreEqual(false, parser.ReadValue("false", typeof (bool), "ignored"));
            Assert.AreEqual(false, parser.ReadValue("False", typeof (bool), "ignored"));
            Assert.AreEqual(false, parser.ReadValue("FALSE", typeof (bool), "ignored"));
            Assert.AreEqual(false, parser.ReadValue("falsE", typeof (bool), "ignored"));
            Assert.AreEqual(false, parser.ReadValue("garbage", typeof (bool), "ignored"));
            Assert.AreEqual(false, parser.ReadValue("", typeof (bool), "ignored"));
            Assert.AreEqual(false, parser.ReadValue(null, typeof (bool), "ignored"));

            var guid = Guid.Parse("{C3CF9078-0122-41BD-9E2D-D3199E937285}");
            Assert.AreEqual(guid,
                            parser.ReadValue("{C3CF9078-0122-41BD-9E2D-D3199E937285}", typeof (Guid),
                                             "ignored"));
            Assert.AreEqual(guid,
                            parser.ReadValue(
                                "{C3CF9078-0122-41BD-9E2D-D3199E937285}".ToLowerInvariant(), typeof (Guid), "ignored"));
            Assert.AreEqual(Guid.Empty,
                            parser.ReadValue("{F9078-0122-41BD-9E2D-D3199E93}", typeof (Guid), "ignored"));
            Assert.AreEqual(Guid.Empty, parser.ReadValue("garbage", typeof (Guid), "ignored"));
            Assert.AreEqual(Guid.Empty, parser.ReadValue("", typeof (Guid), "ignored"));
            Assert.AreEqual(Guid.Empty, parser.ReadValue(null, typeof (Guid), "ignored"));

            Assert.AreEqual(StringSplitOptions.RemoveEmptyEntries,
                            parser.ReadValue("RemoveEmptyEntries", typeof (StringSplitOptions), "ignored"));
            Assert.AreEqual(StringSplitOptions.None,
                            parser.ReadValue("None".ToLowerInvariant(), typeof (StringSplitOptions),
                                             "ignored"));
            Assert.AreEqual(StringSplitOptions.None,
                            parser.ReadValue("garbage", typeof (StringSplitOptions), "ignored"));
            Assert.AreEqual(StringSplitOptions.None,
                            parser.ReadValue("", typeof (StringSplitOptions), "ignored"));
            Assert.AreEqual(StringSplitOptions.None,
                            parser.ReadValue(null, typeof (StringSplitOptions), "ignored"));
        }
    }
}