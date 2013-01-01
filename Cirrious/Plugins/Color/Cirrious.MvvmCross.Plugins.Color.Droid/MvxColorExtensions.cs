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

namespace Cirrious.MvvmCross.Plugins.Color.Droid
{
    public static class MvxColorExtensions
    {
        private static readonly MvxAndroidColor _mvxNativeColor = new MvxAndroidColor();

        public static global::Android.Graphics.Color ToAndroidColor(this MvxColor color)
        {
            return _mvxNativeColor.ToAndroidColor(color);
        }
    }
}