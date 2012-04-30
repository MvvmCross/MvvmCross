#region Copyright
// <copyright file="MvxWindowsPhoneColor.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Windows.Media;
using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.Plugins.Color.WindowsPhone
{
    public class MvxWindowsPhoneColor : IMvxNativeColor
    {
        #region Implementation of IMvxNativeColor

        public object ToNative(MvxColor mvxColor)
        {
            var color = System.Windows.Media.Color.FromArgb((byte)mvxColor.A, (byte)mvxColor.R, (byte)mvxColor.G, (byte)mvxColor.B);
            return new SolidColorBrush(color);
        }

        #endregion
    }
}