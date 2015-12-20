// IMvxEventSourceViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Mac.Views
{
    using System;

    using MvvmCross.Platform.Core;

    public interface IMvxEventSourceViewController : IMvxDisposeSource
    {
        event EventHandler ViewDidLoadCalled;
    }
}