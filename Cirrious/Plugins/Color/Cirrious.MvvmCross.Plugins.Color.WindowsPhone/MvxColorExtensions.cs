#region Copyright

// <copyright file="MvxColorExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

namespace Cirrious.MvvmCross.Plugins.Color.WindowsPhone
{
    public static class MvxColorExtensions
    {
        public static System.Windows.Media.Color ToNativeColor(this MvxColor color)
        {
            return MvxWindowsPhoneColor.ToNativeColor(color);
        }
    }
}