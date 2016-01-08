// MvxInMemoryImageValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Converters;
using System;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MvvmCross.Plugins.PictureChooser.WindowsStore
{
    public class MvxInMemoryImageValueConverter : MvxValueConverter<byte[], BitmapImage>
    {
        protected override BitmapImage Convert(byte[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var image = new BitmapImage();
            using (var randomAccessStream = new InMemoryRandomAccessStream())
            {
#warning one day it would be nice to have a proper async value converter here... something like- http://stackoverflow.com/questions/15003827/async-implementation-of-ivalueconverter - but more built in
                var writeStream = randomAccessStream.AsStreamForWrite();
                writeStream.Write(value, 0, value.Length);
                writeStream.Flush();

                randomAccessStream.Seek(0L);

                image.SetSource(randomAccessStream);
                return image;
            }
        }
    }
}