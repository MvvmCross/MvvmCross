#region Copyright
// <copyright file="MvxAndroidColor.cs" company="Cirrious">
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
    public class MvxAndroidColor : IMvxNativeColor
    {
        #region Implementation of IMvxNativeColor

        public object ToNative(MvxColor mvxColor)
        {
            return ToAndroidColor(mvxColor);
        }

        #endregion

        public global::Android.Graphics.Color ToAndroidColor(MvxColor mvxColor)
        {
            return new global::Android.Graphics.Color(mvxColor.R, mvxColor.G, mvxColor.B, mvxColor.A);
        }
    }
}