// MvxIosTask.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using AppKit;

namespace MvvmCross.Platform.Mac.Platform
{
    public class MvxMacTask
    {
        protected bool DoUrlOpen(NSUrl url)
        {
            var sharedWorkSpace = NSWorkspace.SharedWorkspace;
            return sharedWorkSpace.OpenUrl(url);
        }
    }
}