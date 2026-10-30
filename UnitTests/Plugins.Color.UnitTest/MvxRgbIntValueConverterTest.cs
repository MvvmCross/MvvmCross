// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Plugin.Color;
using MvvmCross.Tests;

namespace MvvmCross.Plugin.Color.UnitTest
{
    [ClassDataSource<MvxTestFixture>(Shared = SharedType.Keyed, Key = "Color")]
    public class MvxRgbIntValueConverterTest : MvxColorValueConverterTest
    {
        public MvxRgbIntValueConverterTest(MvxTestFixture fixture) : base(fixture)
        {
        }

        [Test]
        [Arguments(0xffffff, 0xFFffffff)]
        [Arguments(0x000000, 0xFF000000)]
        [Arguments(0x123456, 0xFF123456)]
        [Arguments(0xA23BCD, 0xFFA23BCD)]
        [Arguments(0x02A040, 0xFF02A040)]
        [Arguments(0x7B02A040, 0xFF02A040)]
        public async Task ConvertRGBIntToColor(int rgb, uint argb)
        {
            var converter = new MvxRGBIntColorValueConverter();
            var actual = converter.Convert(rgb, typeof(object), null, CultureInfo.CurrentUICulture);
            var wrapped = actual as WrappedColor;
            await Assert.That(wrapped).IsNotNull();
            await Assert.That((uint)wrapped.Color.ToArgb()).IsEqualTo(argb);
        }
    }
}
