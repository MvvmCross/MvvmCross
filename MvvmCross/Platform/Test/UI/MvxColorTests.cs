// MvxColorTests.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;
using NUnit.Framework;

namespace Cirrious.CrossCore.Test.UI
{
    [TestFixture]
    public class MvxColorTests
    {
        [Test]
        public void MvxColorSimpleConstructorTests()
        {
            var tests = new uint[,]
                {
                    { 0, 0, 0, 0, 0 },
                    { 0XFF, 0, 0, 0, 0xFF },
                    { 0xFF00, 0, 0, 0xFF, 0 },
                    { 0xFF0000, 0, 0xFF, 0, 0 },
                    { 0xFF000000, 0xFF, 0, 0, 0 },
                    { 0x12345678, 0x12, 0x34, 0x56, 0x78 },
                    { 0xFFFFFFFF, 0xFF, 0xFF, 0xFF, 0xFF },
                };

            for (var i = 0; i < tests.GetUpperBound(0); i++)
            {
                var c = new MvxColor((int)tests[i, 0]);
                Assert.AreEqual(tests[i, 1], c.A);
                Assert.AreEqual(tests[i, 2], c.R);
                Assert.AreEqual(tests[i, 3], c.G);
                Assert.AreEqual(tests[i, 4], c.B);
                Assert.AreEqual(tests[i, 0], (uint)c.ARGB);
            }
        }

        [Test]
        public void MvxColorRGBAndAlphaConstructorTests()
        {
            var tests = new uint[,]
                {
                    { 0, 0, 0, 0 },
                    { 0XFF, 0, 0, 0xFF },
                    { 0xFF00, 0, 0xFF, 0 },
                    { 0xFF0000, 0xFF, 0, 0 },
                    { 0xFF000000, 0, 0, 0 },
                    { 0x12345678, 0x34, 0x56, 0x78 },
                    { 0xFFFFFFFF, 0xFF, 0xFF, 0xFF },
                };

            for (var i = 0; i < tests.GetUpperBound(0); i++)
            {
                for (uint alpha = 0; alpha < 256; alpha++)
                {
                    var c = new MvxColor((int)tests[i, 0], (int)alpha);
                    Assert.AreEqual(alpha, c.A);
                    Assert.AreEqual(tests[i, 1], c.R);
                    Assert.AreEqual(tests[i, 2], c.G);
                    Assert.AreEqual(tests[i, 3], c.B);
                    var argb = (tests[i, 0] & 0x00FFFFFF) | ((alpha & 0xFF) << 24);
                    Assert.AreEqual(argb, (uint)c.ARGB);
                }
            }
        }

        [Test]
        public void MvxColorComponentConstructorTests()
        {
            var tests = new uint[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0xFF },
                    { 0, 0, 0xFF, 0 },
                    { 0, 0xFF, 0, 0 },
                    { 0xFF, 0, 0, 0 },
                    { 0x12, 0x34, 0x56, 0x78 },
                    { 0xFF, 0xFF, 0xFF, 0xFF },
                };

            for (var i = 0; i < tests.GetUpperBound(0); i++)
            {
                var c = new MvxColor((int)tests[i, 1], (int)tests[i, 2], (int)tests[i, 3], (int)tests[i, 0]);
                Assert.AreEqual(tests[i, 0], c.A);
                Assert.AreEqual(tests[i, 1], c.R);
                Assert.AreEqual(tests[i, 2], c.G);
                Assert.AreEqual(tests[i, 3], c.B);
                var argb = tests[i, 0] & 0xFF;
                argb <<= 8;
                argb |= (tests[i, 1] & 0xFF);
                argb <<= 8;
                argb |= (tests[i, 2] & 0xFF);
                argb <<= 8;
                argb |= (tests[i, 3] & 0xFF);
                Assert.AreEqual(argb, (uint)c.ARGB);
            }
        }

        [Test]
        public void MvxAColorComponentTests()
        {
            var tests = new uint[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0xFF },
                    { 0, 0, 0xFF, 0 },
                    { 0, 0xFF, 0, 0 },
                    { 0xFF, 0, 0, 0 },
                    { 0x12, 0x34, 0x56, 0x78 },
                    { 0xFF, 0xFF, 0xFF, 0xFF },
                };

            for (var i = 0; i < tests.GetUpperBound(0); i++)
            {
                var c = new MvxColor((int)tests[i, 1], (int)tests[i, 2], (int)tests[i, 3], (int)tests[i, 0]);
                for (int j = 0; j < 256; j++)
                {
                    c.A = j;

                    Assert.AreEqual(j, c.A);
                    Assert.AreEqual(tests[i, 1], c.R);
                    Assert.AreEqual(tests[i, 2], c.G);
                    Assert.AreEqual(tests[i, 3], c.B);
                    var argb = (uint)j & 0xFF;
                    argb <<= 8;
                    argb |= (tests[i, 1] & 0xFF);
                    argb <<= 8;
                    argb |= (tests[i, 2] & 0xFF);
                    argb <<= 8;
                    argb |= (tests[i, 3] & 0xFF);
                    Assert.AreEqual(argb, (uint)c.ARGB);
                }
            }
        }

        [Test]
        public void MvxRColorComponentTests()
        {
            var tests = new uint[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0xFF },
                    { 0, 0, 0xFF, 0 },
                    { 0, 0xFF, 0, 0 },
                    { 0xFF, 0, 0, 0 },
                    { 0x12, 0x34, 0x56, 0x78 },
                    { 0xFF, 0xFF, 0xFF, 0xFF },
                };

            for (var i = 0; i < tests.GetUpperBound(0); i++)
            {
                var c = new MvxColor((int)tests[i, 1], (int)tests[i, 2], (int)tests[i, 3], (int)tests[i, 0]);
                for (int j = 0; j < 256; j++)
                {
                    c.R = j;

                    Assert.AreEqual(tests[i, 0], c.A);
                    Assert.AreEqual(j, c.R);
                    Assert.AreEqual(tests[i, 2], c.G);
                    Assert.AreEqual(tests[i, 3], c.B);
                    var argb = tests[i, 0] & 0xFF;
                    argb <<= 8;
                    argb |= ((uint)j & 0xFF);
                    argb <<= 8;
                    argb |= (tests[i, 2] & 0xFF);
                    argb <<= 8;
                    argb |= (tests[i, 3] & 0xFF);
                    Assert.AreEqual(argb, (uint)c.ARGB);
                }
            }
        }

        [Test]
        public void MvxGColorComponentTests()
        {
            var tests = new uint[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0xFF },
                    { 0, 0, 0xFF, 0 },
                    { 0, 0xFF, 0, 0 },
                    { 0xFF, 0, 0, 0 },
                    { 0x12, 0x34, 0x56, 0x78 },
                    { 0xFF, 0xFF, 0xFF, 0xFF },
                };

            for (var i = 0; i < tests.GetUpperBound(0); i++)
            {
                var c = new MvxColor((int)tests[i, 1], (int)tests[i, 2], (int)tests[i, 3], (int)tests[i, 0]);
                for (int j = 0; j < 256; j++)
                {
                    c.G = j;

                    Assert.AreEqual(tests[i, 0], c.A);
                    Assert.AreEqual(tests[i, 1], c.R);
                    Assert.AreEqual(j, c.G);
                    Assert.AreEqual(tests[i, 3], c.B);
                    var argb = tests[i, 0] & 0xFF;
                    argb <<= 8;
                    argb |= (tests[i, 1] & 0xFF);
                    argb <<= 8;
                    argb |= ((uint)j & 0xFF);
                    argb <<= 8;
                    argb |= (tests[i, 3] & 0xFF);
                    Assert.AreEqual(argb, (uint)c.ARGB);
                }
            }
        }

        [Test]
        public void MvxBColorComponentTests()
        {
            var tests = new uint[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0xFF },
                    { 0, 0, 0xFF, 0 },
                    { 0, 0xFF, 0, 0 },
                    { 0xFF, 0, 0, 0 },
                    { 0x12, 0x34, 0x56, 0x78 },
                    { 0xFF, 0xFF, 0xFF, 0xFF },
                };

            for (var i = 0; i < tests.GetUpperBound(0); i++)
            {
                var c = new MvxColor((int)tests[i, 1], (int)tests[i, 2], (int)tests[i, 3], (int)tests[i, 0]);
                for (int j = 0; j < 256; j++)
                {
                    c.B = j;

                    Assert.AreEqual(tests[i, 0], c.A);
                    Assert.AreEqual(tests[i, 1], c.R);
                    Assert.AreEqual(tests[i, 2], c.G);
                    Assert.AreEqual(j, c.B);
                    var argb = tests[i, 0] & 0xFF;
                    argb <<= 8;
                    argb |= (tests[i, 1] & 0xFF);
                    argb <<= 8;
                    argb |= (tests[i, 2] & 0xFF);
                    argb <<= 8;
                    argb |= ((uint)j & 0xFF);
                    Assert.AreEqual(argb, (uint)c.ARGB);
                }
            }
        }
    }
}