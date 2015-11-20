// IMvxEventSourceViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using System;

namespace Cirrious.CrossCore.Touch.Views
{
    public interface IMvxEventSourceViewController : IMvxDisposeSource
    {
        event EventHandler ViewDidLoadCalled;

        event EventHandler<MvxValueEventArgs<bool>> ViewWillAppearCalled;

        event EventHandler<MvxValueEventArgs<bool>> ViewDidAppearCalled;

        event EventHandler<MvxValueEventArgs<bool>> ViewDidDisappearCalled;

        event EventHandler<MvxValueEventArgs<bool>> ViewWillDisappearCalled;
    }
}