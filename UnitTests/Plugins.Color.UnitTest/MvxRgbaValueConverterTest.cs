// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Tests;

namespace MvvmCross.Plugin.Color.UnitTest
{
    [ClassDataSource<MvxTestFixture>(Shared = SharedType.Keyed, Key = "Color")]
    public class MvxRgbaValueConverterTest : MvxColorValueConverterTest
    {
        public MvxRgbaValueConverterTest(MvxTestFixture fixture) : base(fixture)
        {
        }

        [Test]
        [Arguments("fff", 0xFFffffff)]
        [Arguments("FFF", 0xFFffffff)]
        [Arguments("000", 0xFF000000)]
        [Arguments("123", 0xFF112233)]
        [Arguments("A23", 0xFFAA2233)]
        [Arguments("02A", 0xFF0022AA)]
        [Arguments("#fff", 0xFFffffff)]
        [Arguments("#FFF", 0xFFffffff)]
        [Arguments("#000", 0xFF000000)]
        [Arguments("#123", 0xFF112233)]
        [Arguments("#A23", 0xFFAA2233)]
        [Arguments("#02A", 0xFF0022AA)]
        [Arguments("ffffff", 0xFFffffff)]
        [Arguments("FFFFFF", 0xFFffffff)]
        [Arguments("000000", 0xFF000000)]
        [Arguments("123456", 0xFF123456)]
        [Arguments("A23BCD", 0xFFA23BCD)]
        [Arguments("02A040", 0xFF02A040)]
        [Arguments("#ffffff", 0xFFffffff)]
        [Arguments("#FFFFFF", 0xFFffffff)]
        [Arguments("#000000", 0xFF000000)]
        [Arguments("#123456", 0xFF123456)]
        [Arguments("#A23BCD", 0xFFA23BCD)]
        [Arguments("#02A040", 0xFF02A040)]
        [Arguments("ffffffff", 0xFFffffff)]
        [Arguments("FFFFFFFF", 0xFFffffff)]
        [Arguments("00000000", 0x00000000)]
        [Arguments("12345678", 0x78123456)]
        [Arguments("A23BCDA9", 0xA9A23BCD)]
        [Arguments("02A04012", 0x1202A040)]
        [Arguments("#ffffffff", 0xFFffffff)]
        [Arguments("#FFFFFFFF", 0xFFffffff)]
        [Arguments("#00000000", 0x00000000)]
        [Arguments("#12345678", 0x78123456)]
        [Arguments("#A23BCDA9", 0xA9A23BCD)]
        [Arguments("#02A04012", 0x1202A040)]
        public async Task TestHexStringToColor(string rgba, uint expected)
        {
            var converter = new MvxRGBAValueConverter();
            var actual = converter.Convert(rgba, typeof(object), null, CultureInfo.CurrentUICulture);
            var wrapped = actual as WrappedColor;
            await Assert.That(wrapped).IsNotNull();
            await Assert.That((uint)wrapped.Color.ToArgb()).IsEqualTo(expected);
        }
    }
}
