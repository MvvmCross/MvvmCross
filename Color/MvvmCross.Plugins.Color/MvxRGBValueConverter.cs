// MvxRGBValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.UI;
using System;
using System.Globalization;

namespace MvvmCross.Plugins.Color
{
    public class MvxRGBValueConverter : MvxColorValueConverter<string>
    {
        protected override MvxColor Convert(string value, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
                return new MvxColor(0);

            value = value.TrimStart('#');
            if (value.Length == 0)
                return new MvxColor(0);

            switch (value.Length)
            {
                case 3:
                    return Parse3DigitColor(value);

                case 6:
                    return Parse6DigitColor(value);

                case 8:
                    return Parse8DigitColor(value);

                default:
                    return new MvxColor(0);
            }
        }

        private MvxColor Parse3DigitColor(string value)
        {
            var red = Int32.Parse(value.Substring(0, 1), NumberStyles.HexNumber);
            var green = Int32.Parse(value.Substring(1, 1), NumberStyles.HexNumber);
            var blue = Int32.Parse(value.Substring(2, 1), NumberStyles.HexNumber);
            return new MvxColor(UpByte(red), UpByte(green), UpByte(blue));
        }

        private int UpByte(int input)
        {
            var fourBit = input & 0xF;
            var output = fourBit << 4;
            output |= fourBit;
            return output;
        }

        private MvxColor Parse6DigitColor(string value)
        {
            var rgb = Int32.Parse(value, NumberStyles.HexNumber);
            return new MvxColor(rgb, 255);
        }

        protected virtual MvxColor Parse8DigitColor(string value)
        {
            // assume RGBA
            var rgb = Int32.Parse(value.Substring(0, 6), NumberStyles.HexNumber);
            var a = Int32.Parse(value.Substring(6, 2), NumberStyles.HexNumber);
            return new MvxColor(rgb, a);
        }
    }
}