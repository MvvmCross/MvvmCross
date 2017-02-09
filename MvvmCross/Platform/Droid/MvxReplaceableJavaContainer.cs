// MvxReplaceableJavaContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid
{
    public class MvxReplaceableJavaContainer : Java.Lang.Object
    {
        public object Object { get; set; }

        public override string ToString()
        {
            return this.Object?.ToString() ?? string.Empty;
        }
    }
}