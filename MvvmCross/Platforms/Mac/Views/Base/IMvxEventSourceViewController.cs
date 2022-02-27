// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Mac.Views.Base
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
