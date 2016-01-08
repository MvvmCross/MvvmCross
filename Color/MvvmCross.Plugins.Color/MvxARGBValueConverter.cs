// MvxARGBValueConverter.cs
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
    public class MvxARGBValueConverter : MvxRGBValueConverter
    {
        protected override MvxColor Parse8DigitColor(string value)
        {
            var a = Int32.Parse(value.Substring(0, 2), NumberStyles.HexNumber);
            var rgb = Int32.Parse(value.Substring(2, 6), NumberStyles.HexNumber);
            return new MvxColor(rgb, a);
        }
    }
}