// IMvxNativeColor.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.UI
{
    public interface IMvxNativeColor
    {
        object ToNative(MvxColor mvxColor);
    }
}