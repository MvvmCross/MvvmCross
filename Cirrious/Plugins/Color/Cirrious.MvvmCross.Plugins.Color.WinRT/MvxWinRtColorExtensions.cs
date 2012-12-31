#region Copyright

// <copyright file="MvxWinRtColorExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

namespace Cirrious.MvvmCross.Plugins.Color.WinRT
{
    public static class MvxWinRtColorExtensions
    {
        public static Windows.UI.Color ToNativeColor(this MvxColor mvxColor)
        {
            var color = Windows.UI.Color.FromArgb((byte) mvxColor.A,
                                                  (byte) mvxColor.R,
                                                  (byte) mvxColor.G,
                                                  (byte) mvxColor.B);
            return color;
        }
    }
}