using System;
using System.Windows.Media;

namespace MvvmCross.Plugins.PictureChooser.Wpf
{
    /// <summary>
    /// Convertion class for PixelFormat between two namespaces
    /// Code copied from https://github.com/mathieumack/PixelFormatsConverter
    /// </summary>
    internal static class PixelFormatConverterExtensions
    {
        /// <summary>
        /// Convert from System.Drawing.Imaging.PixelFormat to System.Windows.Media.PixelFormat
        /// </summary>
        /// <param name="pixelFormat"></param>
        /// <exception cref="NotSupportedException">Convertion is not available</exception>
        /// <returns></returns>
        public static PixelFormat Convert(this System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppGrayScale)
                return PixelFormats.Gray16;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppRgb555)
                return PixelFormats.Bgr555;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppRgb565)
                return PixelFormats.Bgr565;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Indexed)
                return PixelFormats.Bgr101010;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
                return PixelFormats.Indexed1;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed)
                return PixelFormats.Indexed4;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                return PixelFormats.Indexed8;
            
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                return PixelFormats.Bgr24;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                return PixelFormats.Bgr32;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
                return PixelFormats.Pbgra32;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                return PixelFormats.Bgr32;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format48bppRgb)
                return PixelFormats.Rgb48;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format64bppArgb)
                return PixelFormats.Prgba64;
            
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Undefined)
                return PixelFormats.Default;

            throw new NotSupportedException("Convertion not supported with " + pixelFormat.ToString());
        }
    }
}
