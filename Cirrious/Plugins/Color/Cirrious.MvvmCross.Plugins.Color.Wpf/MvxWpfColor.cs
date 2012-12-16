#region Copyright
// <copyright file="MvxWpfColor.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Windows.Media;

namespace Cirrious.MvvmCross.Plugins.Color.Wpf
{
    public class MvxWpfColor : IMvxNativeColor
    {
        #region Implementation of IMvxNativeColor

        public object ToNative(MvxColor mvxColor)
        {
            var color = ToNativeColor(mvxColor);
            return new SolidColorBrush(color);
        }

        #endregion

        public static System.Windows.Media.Color ToNativeColor(MvxColor mvxColor)
        {
            return System.Windows.Media.Color.FromArgb((byte)mvxColor.A, (byte)mvxColor.R, (byte)mvxColor.G, (byte)mvxColor.B);
        }
    }
}