// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using MvvmCross.Converters;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Uap
{
    [Preserve(AllMembers = true)]
    public class MvxInMemoryImageValueConverter : MvxValueConverter<byte[], BitmapImage>
    {
        protected override BitmapImage Convert(byte[] value, Type targetType, object parameter, CultureInfo culture)
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
