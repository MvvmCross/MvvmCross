// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Foundation;
using MvvmCross.Converters;
using UIKit;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Ios
{
    [Preserve(AllMembers = true)]
    public class MvxInMemoryImageValueConverter : MvxValueConverter<byte[], UIImage>
    {
        protected override UIImage Convert(byte[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var imageData = NSData.FromArray(value);
            return UIImage.LoadFromData(imageData);
        }
    }
}
