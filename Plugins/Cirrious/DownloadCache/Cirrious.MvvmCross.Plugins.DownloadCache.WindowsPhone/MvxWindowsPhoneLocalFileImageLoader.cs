using System;
using System.IO;
using System.Windows;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.WindowsPhone
{
    public class MvxWindowsPhoneLocalFileImageLoader
        : IMvxLocalFileImageLoader<byte[]>
    {
        private const string ResourcePrefix = "res:";

        public MvxImage<byte[]> Load(string localPath, bool shouldCache /* ignored here */)
        {
            byte[] bitmap;
            if (localPath.StartsWith(ResourcePrefix))
            {
                var resourcePath = localPath.Substring(ResourcePrefix.Length);
                bitmap = LoadResourceImage(resourcePath);
            }
            else
            {
                bitmap = LoadBitmapImage(localPath);
            }
            return new MvxWindowsPhoneImage(bitmap);
        }

        private byte[] LoadBitmapImage(string localPath)
        {
            var file = Mvx.Resolve<IMvxFileStore>();

            byte[] bitmap = null;
            if (!file.TryReadBinaryFile(localPath, stream =>
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    bitmap = memoryStream.ToArray();
                }
                return true;
            }))
                return null;

            return bitmap;
        }

        private byte[] LoadResourceImage(string resourcePath)
        {
            byte[] bitmap;
            var streamInfo = Application.GetResourceStream(new Uri(resourcePath, UriKind.Relative));

            using (var memoryStream = new MemoryStream())
            {
                streamInfo.Stream.CopyTo(memoryStream);
                bitmap = memoryStream.ToArray();
            }

            return bitmap;
        }
    }
}