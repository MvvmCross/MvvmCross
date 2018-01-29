// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.UI;
using Xunit;

namespace MvvmCross.Platform.Test.UI
{
    
    public class MvxColorTests
    {
        [Fact]
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
                Assert.Equal(tests[i, 1], c.A);
                Assert.Equal(tests[i, 2], c.R);
                Assert.Equal(tests[i, 3], c.G);
                Assert.Equal(tests[i, 4], c.B);
                Assert.Equal(tests[i, 0], (uint)c.ARGB);
            }
        }

        [Fact]
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
                    Assert.Equal(alpha, c.A);
                    Assert.Equal(tests[i, 1], c.R);
                    Assert.Equal(tests[i, 2], c.G);
                    Assert.Equal(tests[i, 3], c.B);
                    var argb = (tests[i, 0] & 0x00FFFFFF) | ((alpha & 0xFF) << 24);
                    Assert.Equal(argb, (uint)c.ARGB);
                }
            }
        }

        [Fact]
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
                Assert.Equal(tests[i, 0], c.A);
                Assert.Equal(tests[i, 1], c.R);
                Assert.Equal(tests[i, 2], c.G);
                Assert.Equal(tests[i, 3], c.B);
                var argb = tests[i, 0] & 0xFF;
                argb <<= 8;
                argb |= tests[i, 1] & 0xFF;
                argb <<= 8;
                argb |= tests[i, 2] & 0xFF;
                argb <<= 8;
                argb |= tests[i, 3] & 0xFF;
                Assert.Equal(argb, (uint)c.ARGB);
            }
        }

        [Fact]
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

                    Assert.Equal(j, c.A);
                    Assert.Equal(tests[i, 1], c.R);
                    Assert.Equal(tests[i, 2], c.G);
                    Assert.Equal(tests[i, 3], c.B);
                    var argb = (uint)j & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 1] & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 2] & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 3] & 0xFF;
                    Assert.Equal(argb, (uint)c.ARGB);
                }
            }
        }

        [Fact]
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

                    Assert.Equal(tests[i, 0], c.A);
                    Assert.Equal(j, c.R);
                    Assert.Equal(tests[i, 2], c.G);
                    Assert.Equal(tests[i, 3], c.B);
                    var argb = tests[i, 0] & 0xFF;
                    argb <<= 8;
                    argb |= (uint)j & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 2] & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 3] & 0xFF;
                    Assert.Equal(argb, (uint)c.ARGB);
                }
            }
        }

        [Fact]
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

                    Assert.Equal(tests[i, 0], c.A);
                    Assert.Equal(tests[i, 1], c.R);
                    Assert.Equal(j, c.G);
                    Assert.Equal(tests[i, 3], c.B);
                    var argb = tests[i, 0] & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 1] & 0xFF;
                    argb <<= 8;
                    argb |= (uint)j & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 3] & 0xFF;
                    Assert.Equal(argb, (uint)c.ARGB);
                }
            }
        }

        [Fact]
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

                    Assert.Equal(tests[i, 0], c.A);
                    Assert.Equal(tests[i, 1], c.R);
                    Assert.Equal(tests[i, 2], c.G);
                    Assert.Equal(j, c.B);
                    var argb = tests[i, 0] & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 1] & 0xFF;
                    argb <<= 8;
                    argb |= tests[i, 2] & 0xFF;
                    argb <<= 8;
                    argb |= (uint)j & 0xFF;
                    Assert.Equal(argb, (uint)c.ARGB);
                }
            }
        }
    }
}