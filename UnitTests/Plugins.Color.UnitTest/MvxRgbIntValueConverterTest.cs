// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Plugin.Color;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.Plugins.Color.UnitTest
{
    [Collection("Color")]
    public class MvxRgbIntValueConverterTest : MvxColorValueConverterTest
    {
        public MvxRgbIntValueConverterTest(MvxTestFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData(0xffffff, 0xFFffffff)]
        [InlineData(0x000000, 0xFF000000)]
        [InlineData(0x123456, 0xFF123456)]
        [InlineData(0xA23BCD, 0xFFA23BCD)]
        [InlineData(0x02A040, 0xFF02A040)]
        [InlineData(0x7B02A040, 0xFF02A040)]
        public void ConvertRGBIntToColor(int rgb, uint argb)
        {
            var converter = new MvxRGBIntColorValueConverter();
            var actual = converter.Convert(rgb, typeof(object), null, CultureInfo.CurrentUICulture);
            var wrapped = actual as WrappedColor;
            Assert.NotNull(wrapped);
            Assert.Equal(argb, (uint)wrapped.Color.ARGB);
        }
    }
}
