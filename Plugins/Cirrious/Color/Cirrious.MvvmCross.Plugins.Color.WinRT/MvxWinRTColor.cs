// MvxWinRTColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Windows.UI.Xaml.Media;

namespace Cirrious.MvvmCross.Plugins.Color.WinRT
{
    public class MvxWinRTColor : IMvxNativeColor
    {
        #region Implementation of IMvxNativeColor

        public object ToNative(MvxColor mvxColor)
        {
            var color = mvxColor.ToNativeColor();
            return new SolidColorBrush(color);
        }

        #endregion
    }
}