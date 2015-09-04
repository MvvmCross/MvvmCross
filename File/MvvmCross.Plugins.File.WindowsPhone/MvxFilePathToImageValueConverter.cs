// MvxFilePathToImageValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Media.Imaging;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;

namespace MvvmCross.Plugins.File.WindowsPhone
{
    public class MvxFilePathToImageValueConverter : MvxValueConverter<string>
    {
        protected override object Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var fileStore = Mvx.Resolve<IMvxFileStore>();

            var bm = new BitmapImage();
            fileStore.TryReadBinaryFile((string)value, (stream) =>
                {
                    bm.SetSource(stream);
                    return true;
                });

            return bm;
        }
    }
}