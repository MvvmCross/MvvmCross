#region Copyright
// <copyright file="MvxTouchColor.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.Color.Touch
{
    public class MvxTouchColor : IMvxNativeColor
    {
        #region Implementation of IMvxNativeColor

        public object ToNative(MvxColor mvxColor)
        {
            return ToUIColor(mvxColor);
        }

        #endregion

        public static UIColor ToUIColor(MvxColor mvxColor)
        {
            return new MonoTouch.UIKit.UIColor(mvxColor.R, mvxColor.G, mvxColor.B, mvxColor.A);
        }
    }
}