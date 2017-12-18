// IMvxApplicationDelegate.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using AppKit;
using MvvmCross.Core.Platform;

namespace MvvmCross.Mac.Platform
{
    public interface IMvxApplicationDelegate : INSApplicationDelegate, IMvxLifetime
    {
    }
}