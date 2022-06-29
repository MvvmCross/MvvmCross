// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.Plugin.Color.UnitTest
{
    [Collection("Color")]
    public class MvxRgbaValueConverterTest : MvxColorValueConverterTest
    {
        public MvxRgbaValueConverterTest(MvxTestFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("fff", 0xFFffffff)]
        [InlineData("FFF", 0xFFffffff)]
        [InlineData("000", 0xFF000000)]
        [InlineData("123", 0xFF112233)]
        [InlineData("A23", 0xFFAA2233)]
        [InlineData("02A", 0xFF0022AA)]
        [InlineData("#fff", 0xFFffffff)]
        [InlineData("#FFF", 0xFFffffff)]
        [InlineData("#000", 0xFF000000)]
        [InlineData("#123", 0xFF112233)]
        [InlineData("#A23", 0xFFAA2233)]
        [InlineData("#02A", 0xFF0022AA)]
        [InlineData("ffffff", 0xFFffffff)]
        [InlineData("FFFFFF", 0xFFffffff)]
        [InlineData("000000", 0xFF000000)]
        [InlineData("123456", 0xFF123456)]
        [InlineData("A23BCD", 0xFFA23BCD)]
        [InlineData("02A040", 0xFF02A040)]
        [InlineData("#ffffff", 0xFFffffff)]
        [InlineData("#FFFFFF", 0xFFffffff)]
        [InlineData("#000000", 0xFF000000)]
        [InlineData("#123456", 0xFF123456)]
        [InlineData("#A23BCD", 0xFFA23BCD)]
        [InlineData("#02A040", 0xFF02A040)]
        [InlineData("ffffffff", 0xFFffffff)]
        [InlineData("FFFFFFFF", 0xFFffffff)]
        [InlineData("00000000", 0x00000000)]
        [InlineData("12345678", 0x78123456)]
        [InlineData("A23BCDA9", 0xA9A23BCD)]
        [InlineData("02A04012", 0x1202A040)]
        [InlineData("#ffffffff", 0xFFffffff)]
        [InlineData("#FFFFFFFF", 0xFFffffff)]
        [InlineData("#00000000", 0x00000000)]
        [InlineData("#12345678", 0x78123456)]
        [InlineData("#A23BCDA9", 0xA9A23BCD)]
        [InlineData("#02A04012", 0x1202A040)]
        public void TestHexStringToColor(string rgba, uint expected)
        {
            var converter = new MvxRGBAValueConverter();
            var actual = converter.Convert(rgba, typeof(object), null, CultureInfo.CurrentUICulture);
            var wrapped = actual as WrappedColor;
            Assert.NotNull(wrapped);
            Assert.Equal(expected, (uint)wrapped.Color.ToArgb());
        }
    }
}
