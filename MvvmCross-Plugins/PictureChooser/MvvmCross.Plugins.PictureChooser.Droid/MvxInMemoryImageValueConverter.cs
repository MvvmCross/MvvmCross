﻿// MvxInMemoryImageValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Android.Graphics;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Plugins.PictureChooser.Droid
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