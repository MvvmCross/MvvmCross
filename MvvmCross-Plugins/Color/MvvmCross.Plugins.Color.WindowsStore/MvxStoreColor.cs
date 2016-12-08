// MvxStoreColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.UI;
using Windows.UI.Xaml.Media;

namespace MvvmCross.Plugins.Color.WindowsStore
{
    public class MvxStoreColor : IMvxNativeColor
    {
        public object ToNative(MvxColor mvxColor)
        {
            var color = mvxColor.ToNativeColor();
            return new SolidColorBrush(color);
        }
    }
}