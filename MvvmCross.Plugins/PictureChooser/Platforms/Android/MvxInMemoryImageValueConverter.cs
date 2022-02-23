// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Android.Graphics;
using MvvmCross.Converters;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Android
{
    [Preserve(AllMembers = true)]
    public class MvxInMemoryImageValueConverter : MvxValueConverter<byte[], Bitmap>
    {
        protected override Bitmap Convert(byte[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            // the InPurgeable option is very important for Droid memory management.
            // see http://slodge.blogspot.co.uk/2013/02/huge-android-memory-bug-and-bug-hunting.html
            var options = new BitmapFactory.Options() { InPurgeable = true };
            var image = BitmapFactory.DecodeByteArray(value, 0, value.Length, options);
            return image;
        }
    }
}
