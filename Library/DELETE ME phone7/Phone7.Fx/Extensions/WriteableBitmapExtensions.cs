using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Phone7.Fx.Extensions
{
    public static class WriteableBitmapExtensions
    {
        public static Color GetPixel(this WriteableBitmap bmp, int x, int y)
        {
            int i = bmp.Pixels[y * bmp.PixelWidth + x];
            return Color.FromArgb(255, (byte)((i >> 16) & 0xFF), (byte)((i >> 8) & 0xFF), (byte)(i & 0xFF));

        }

        public static WriteableBitmap FromResource(this WriteableBitmap bmp, string relativePath)
        {
            if (bmp == null) 
                throw new ArgumentNullException("bmp");
            var fullName = System.Reflection.Assembly.GetCallingAssembly().FullName;
            var asmName = new System.Reflection.AssemblyName(fullName).Name;
            using (var bmpStream = Application.GetResourceStream(new Uri(string.Format("{0};component/{1}", asmName, relativePath), UriKind.Relative)).Stream)
            {
                var bmpi = new BitmapImage();
                bmpi.SetSource(bmpStream);
                bmp = new WriteableBitmap(bmpi);
                return bmp;
            }
        }


    }
}