// MvxInMemoryImageValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.WindowsStore.Platform;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.Touch
{
    public class MvxInMemoryImageValueConverter : MvxValueConverter<byte[], BitmapImage>
    {
        protected override BitmapImage Convert(byte[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var image = new BitmapImage();
            using(var randomAccessStream = new InMemoryRandomAccessStream())
            {
#warning one day it would be nice to have a proper async value converter here... something like- http://stackoverflow.com/questions/15003827/async-implementation-of-ivalueconverter - but more built in
                var writeStream = randomAccessStream.AsStreamForWrite();
                var task = writeStream.WriteAsync(value, 0, value.Length);
                task.RunSynchronously();
                task = writeStream.FlushAsync();
                task.RunSynchronously();
 
                randomAccessStream.Seek(0L);

                image.SetSourceAsync(randomAccessStream).Await();
                return image;
            }
        }
    }
}