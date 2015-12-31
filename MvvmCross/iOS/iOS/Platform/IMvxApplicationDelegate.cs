// IMvxApplicationDelegate.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Platform
{
    using MvvmCross.Core.Platform;

    using UIKit;

    public interface IMvxApplicationDelegate : IUIApplicationDelegate, IMvxLifetime
    {
    }
}