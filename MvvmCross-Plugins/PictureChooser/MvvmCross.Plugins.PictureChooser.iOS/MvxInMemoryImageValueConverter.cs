// MvxInMemoryImageValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Foundation;
using UIKit;

namespace MvvmCross.Plugins.PictureChooser.iOS
{
    [All.Preserve(AllMembers = true)]
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