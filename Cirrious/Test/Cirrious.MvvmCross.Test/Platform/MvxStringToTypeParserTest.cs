// MvxStringToTypeParserTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.ViewModels;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test.Platform
{
    [TestFixture]
    public class MvxStringToTypeParserTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_AllTypesAreSupported()
        {
            Assert.IsTrue(MvxStringToTypeParser.TypeSupported(typeof (string)));
            Assert.IsTrue(MvxStringToTypeParser.TypeSupported(typeof (int)));
            Assert.IsTrue(MvxStringToTypeParser.TypeSupported(typeof (long)));
            Assert.IsTrue(MvxStringToTypeParser.TypeSupported(typeof (double)));
            Assert.IsTrue(MvxStringToTypeParser.TypeSupported(typeof (bool)));
            Assert.IsTrue(MvxStringToTypeParser.TypeSupported(typeof (Guid)));
            Assert.IsTrue(MvxStringToTypeParser.TypeSupported(typeof (StringSplitOptions)));
        }

        [Test]
        public void Test_AllTypesCanBeRead()
        {
            Assert.AreEqual("Hello World", MvxStringToTypeParser.ReadValue("Hello World", typeof (string), "ignored"));
            Assert.AreEqual("", MvxStringToTypeParser.ReadValue("", typeof (string), "ignored"));
            Assert.AreEqual(null, MvxStringToTypeParser.ReadValue(null, typeof (string), "ignored"));

            Assert.AreEqual(1.23, MvxStringToTypeParser.ReadValue("1.23", typeof (double), "ignored"));
            Assert.AreEqual(123.0, MvxStringToTypeParser.ReadValue("1,23", typeof (double), "ignored"),
                            "comma separators ignored under invariant parsing");
            Assert.AreEqual(0, MvxStringToTypeParser.ReadValue("garbage", typeof (double), "ignored"));
            Assert.AreEqual(0, MvxStringToTypeParser.ReadValue("", typeof (double), "ignored"));
            Assert.AreEqual(0, MvxStringToTypeParser.ReadValue(null, typeof (double), "ignored"));

            Assert.AreEqual(123, MvxStringToTypeParser.ReadValue("123", typeof (int), "ignored"));
            Assert.AreEqual(0, MvxStringToTypeParser.ReadValue("12.3", typeof (int), "ignored"),
                            "partial integers should not be parsed");
            Assert.AreEqual(0, MvxStringToTypeParser.ReadValue("garbage", typeof (int), "ignored"));
            Assert.AreEqual(0, MvxStringToTypeParser.ReadValue("", typeof (int), "ignored"));
            Assert.AreEqual(0, MvxStringToTypeParser.ReadValue(null, typeof (int), "ignored"));

            Assert.AreEqual(123L, MvxStringToTypeParser.ReadValue("123", typeof (long), "ignored"));
            Assert.AreEqual(0, MvxStringToTypeParser.ReadValue("12.3", typeof (long), "ignored"),
                            "partial integers should not be parsed");
            Assert.AreEqual(0L, MvxStringToTypeParser.ReadValue("garbage", typeof (long), "ignored"));
            Assert.AreEqual(0L, MvxStringToTypeParser.ReadValue("", typeof (long), "ignored"));
            Assert.AreEqual(0L, MvxStringToTypeParser.ReadValue(null, typeof (long), "ignored"));

            Assert.AreEqual(true, MvxStringToTypeParser.ReadValue("true", typeof (bool), "ignored"));
            Assert.AreEqual(true, MvxStringToTypeParser.ReadValue("True", typeof (bool), "ignored"));
            Assert.AreEqual(true, MvxStringToTypeParser.ReadValue("TRUE", typeof (bool), "ignored"));
            Assert.AreEqual(true, MvxStringToTypeParser.ReadValue("truE", typeof (bool), "ignored"));
            Assert.AreEqual(false, MvxStringToTypeParser.ReadValue("false", typeof (bool), "ignored"));
            Assert.AreEqual(false, MvxStringToTypeParser.ReadValue("False", typeof (bool), "ignored"));
            Assert.AreEqual(false, MvxStringToTypeParser.ReadValue("FALSE", typeof (bool), "ignored"));
            Assert.AreEqual(false, MvxStringToTypeParser.ReadValue("falsE", typeof (bool), "ignored"));
            Assert.AreEqual(false, MvxStringToTypeParser.ReadValue("garbage", typeof (bool), "ignored"));
            Assert.AreEqual(false, MvxStringToTypeParser.ReadValue("", typeof (bool), "ignored"));
            Assert.AreEqual(false, MvxStringToTypeParser.ReadValue(null, typeof (bool), "ignored"));

            var guid = Guid.Parse("{C3CF9078-0122-41BD-9E2D-D3199E937285}");
            Assert.AreEqual(guid,
                            MvxStringToTypeParser.ReadValue("{C3CF9078-0122-41BD-9E2D-D3199E937285}", typeof (Guid),
                                                            "ignored"));
            Assert.AreEqual(guid,
                            MvxStringToTypeParser.ReadValue(
                                "{C3CF9078-0122-41BD-9E2D-D3199E937285}".ToLowerInvariant(), typeof (Guid), "ignored"));
            Assert.AreEqual(Guid.Empty,
                            MvxStringToTypeParser.ReadValue("{F9078-0122-41BD-9E2D-D3199E93}", typeof (Guid), "ignored"));
            Assert.AreEqual(Guid.Empty, MvxStringToTypeParser.ReadValue("garbage", typeof (Guid), "ignored"));
            Assert.AreEqual(Guid.Empty, MvxStringToTypeParser.ReadValue("", typeof (Guid), "ignored"));
            Assert.AreEqual(Guid.Empty, MvxStringToTypeParser.ReadValue(null, typeof (Guid), "ignored"));

            Assert.AreEqual(StringSplitOptions.RemoveEmptyEntries,
                            MvxStringToTypeParser.ReadValue("RemoveEmptyEntries", typeof (StringSplitOptions), "ignored"));
            Assert.AreEqual(StringSplitOptions.None,
                            MvxStringToTypeParser.ReadValue("None".ToLowerInvariant(), typeof (StringSplitOptions),
                                                            "ignored"));
            Assert.AreEqual(StringSplitOptions.None,
                            MvxStringToTypeParser.ReadValue("garbage", typeof (StringSplitOptions), "ignored"));
            Assert.AreEqual(StringSplitOptions.None,
                            MvxStringToTypeParser.ReadValue("", typeof (StringSplitOptions), "ignored"));
            Assert.AreEqual(StringSplitOptions.None,
                            MvxStringToTypeParser.ReadValue(null, typeof (StringSplitOptions), "ignored"));
        }
    }
}