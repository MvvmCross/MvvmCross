using System;

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
        public static System.Windows.Media.PixelFormat Convert(this System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppGrayScale)
                return System.Windows.Media.PixelFormats.Gray16;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppRgb555)
                return System.Windows.Media.PixelFormats.Bgr555;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppRgb565)
                return System.Windows.Media.PixelFormats.Bgr565;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Indexed)
                return System.Windows.Media.PixelFormats.Bgr101010;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
                return System.Windows.Media.PixelFormats.Indexed1;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed)
                return System.Windows.Media.PixelFormats.Indexed4;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                return System.Windows.Media.PixelFormats.Indexed8;
            
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                return System.Windows.Media.PixelFormats.Bgr24;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                return System.Windows.Media.PixelFormats.Bgr32;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
                return System.Windows.Media.PixelFormats.Pbgra32;
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                return System.Windows.Media.PixelFormats.Bgr32;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format48bppRgb)
                return System.Windows.Media.PixelFormats.Rgb48;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format64bppArgb)
                return System.Windows.Media.PixelFormats.Prgba64;
            
            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Undefined)
                return System.Windows.Media.PixelFormats.Default;

            throw new NotSupportedException("Convertion not supported with " + pixelFormat.ToString());
        }

        /// <summary>
        /// Convert from System.Windows.Media.PixelFormat to System.Drawing.Imaging.PixelFormat
        /// </summary>
        /// <param name="pixelFormat"></param>
        /// <exception cref="NotSupportedException">Convertion is not available</exception>
        /// <returns></returns>
        public static System.Drawing.Imaging.PixelFormat Convert(this System.Windows.Media.PixelFormat pixelFormat)
        {
            if (pixelFormat == System.Windows.Media.PixelFormats.Gray16)
                return System.Drawing.Imaging.PixelFormat.Format16bppGrayScale;
            if (pixelFormat == System.Windows.Media.PixelFormats.Bgr555)
                return System.Drawing.Imaging.PixelFormat.Format16bppRgb555;
            if (pixelFormat == System.Windows.Media.PixelFormats.Bgr565)
                return System.Drawing.Imaging.PixelFormat.Format16bppRgb565;

            if (pixelFormat == System.Windows.Media.PixelFormats.Bgr101010)
                return System.Drawing.Imaging.PixelFormat.Indexed;
            if (pixelFormat == System.Windows.Media.PixelFormats.Indexed1)
                return System.Drawing.Imaging.PixelFormat.Format1bppIndexed;
            if (pixelFormat == System.Windows.Media.PixelFormats.Indexed4)
                return System.Drawing.Imaging.PixelFormat.Format4bppIndexed;
            if (pixelFormat == System.Windows.Media.PixelFormats.Indexed8)
                return System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
            
            if (pixelFormat == System.Windows.Media.PixelFormats.Bgr24)
                return System.Drawing.Imaging.PixelFormat.Format24bppRgb;

            if (pixelFormat == System.Windows.Media.PixelFormats.Bgr32)
                return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            if (pixelFormat == System.Windows.Media.PixelFormats.Pbgra32)
                return System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
            if (pixelFormat == System.Windows.Media.PixelFormats.Bgr32)
                return System.Drawing.Imaging.PixelFormat.Format32bppRgb;

            if (pixelFormat == System.Windows.Media.PixelFormats.Rgb48)
                return System.Drawing.Imaging.PixelFormat.Format48bppRgb;

            if (pixelFormat == System.Windows.Media.PixelFormats.Prgba64)
                return System.Drawing.Imaging.PixelFormat.Format64bppArgb;
            
            if (pixelFormat == System.Windows.Media.PixelFormats.Default)
                return System.Drawing.Imaging.PixelFormat.Undefined;

            throw new NotSupportedException("Convertion not supported with " + pixelFormat.ToString());
        }
    }
}
