// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using PixelFormats = System.Windows.Media.PixelFormats;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Wpf
{
    /// <summary>
    /// Convertion class for PixelFormat between two namespaces
    /// Code copied from https://github.com/mathieumack/PixelFormatsConverter
    /// </summary>
    internal static class PixelFormatConverterExtensions
    {
        /// <summary>
        /// Convert from PixelFormat to System.Windows.Media.PixelFormat
        /// </summary>
        /// <param name="pixelFormat"></param>
        /// <exception cref="NotSupportedException">Convertion is not available</exception>
        /// <returns></returns>
        public static System.Windows.Media.PixelFormat Convert(this PixelFormat pixelFormat)
        {
            if (pixelFormat == PixelFormat.Format16bppGrayScale)
                return PixelFormats.Gray16;
            if (pixelFormat == PixelFormat.Format16bppRgb555)
                return PixelFormats.Bgr555;
            if (pixelFormat == PixelFormat.Format16bppRgb565)
                return PixelFormats.Bgr565;

            if (pixelFormat == PixelFormat.Indexed)
                return PixelFormats.Bgr101010;
            if (pixelFormat == PixelFormat.Format1bppIndexed)
                return PixelFormats.Indexed1;
            if (pixelFormat == PixelFormat.Format4bppIndexed)
                return PixelFormats.Indexed4;
            if (pixelFormat == PixelFormat.Format8bppIndexed)
                return PixelFormats.Indexed8;

            if (pixelFormat == PixelFormat.Format24bppRgb)
                return PixelFormats.Bgr24;

            if (pixelFormat == PixelFormat.Format32bppArgb)
                return PixelFormats.Bgr32;
            if (pixelFormat == PixelFormat.Format32bppPArgb)
                return PixelFormats.Pbgra32;
            if (pixelFormat == PixelFormat.Format32bppRgb)
                return PixelFormats.Bgr32;

            if (pixelFormat == PixelFormat.Format48bppRgb)
                return PixelFormats.Rgb48;

            if (pixelFormat == PixelFormat.Format64bppArgb)
                return PixelFormats.Prgba64;

            if (pixelFormat == PixelFormat.Undefined)
                return PixelFormats.Default;

            throw new NotSupportedException("Convertion not supported with " + pixelFormat.ToString());
        }
    }
}
