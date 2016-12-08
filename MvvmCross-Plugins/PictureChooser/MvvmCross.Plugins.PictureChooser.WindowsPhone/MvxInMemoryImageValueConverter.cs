// MvxInMemoryImageValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Converters;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace MvvmCross.Plugins.PictureChooser.WindowsPhone
{
    public class MvxInMemoryImageValueConverter : MvxValueConverter<byte[], BitmapImage>
    {
        protected override BitmapImage Convert(byte[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var image = new BitmapImage();
            var memoryStream = new MemoryStream(value);
            image.SetSource(memoryStream);
            return image;
        }
    }
}