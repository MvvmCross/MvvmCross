// MvxRGBAValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.CrossCore.UI;

namespace Cirrious.MvvmCross.Plugins.Color
{
    public class MvxRGBAValueConverter : MvxColorValueConverter<string>
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
            var red = Int32.Parse(value.Substring(0, 1), NumberStyles.HexNumber) * 16;
            var green = Int32.Parse(value.Substring(1, 1), NumberStyles.HexNumber) * 16;
            var blue = Int32.Parse(value.Substring(2, 1), NumberStyles.HexNumber) * 16;
            return new MvxColor(red, green, blue);
        }

        private MvxColor Parse6DigitColor(string value)
        {
            var rgb = Int32.Parse(value, NumberStyles.HexNumber);
            return new MvxColor(rgb);
        }

        private MvxColor Parse8DigitColor(string value)
        {
            var rgb = Int32.Parse(value.Substring(0,6), NumberStyles.HexNumber);
            var a = Int32.Parse(value.Substring(6, 2), NumberStyles.HexNumber);
            return new MvxColor(rgb, a);
        }
    }
}