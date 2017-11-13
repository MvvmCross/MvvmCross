// IMvxEventSourceViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Core;

namespace MvvmCross.Platform.Mac.Views
{
    public interface IMvxEventSourceViewController : IMvxDisposeSource
    {
        event EventHandler ViewDidLoadCalled;

        event EventHandler ViewDidLayoutCalled;

        event EventHandler ViewWillAppearCalled;

        event EventHandler ViewDidAppearCalled;

        event EventHandler ViewDidDisappearCalled;

        event EventHandler ViewWillDisappearCalled;
    }
}