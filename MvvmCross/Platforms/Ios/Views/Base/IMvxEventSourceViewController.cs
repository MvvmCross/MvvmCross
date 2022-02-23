// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Ios.Views.Base
{
    public interface IMvxEventSourceViewController : IMvxDisposeSource
    {
        event EventHandler ViewDidLoadCalled;

        event EventHandler ViewDidLayoutSubviewsCalled;

        event EventHandler<MvxValueEventArgs<bool>> ViewWillAppearCalled;

        event EventHandler<MvxValueEventArgs<bool>> ViewDidAppearCalled;

        event EventHandler<MvxValueEventArgs<bool>> ViewDidDisappearCalled;

        event EventHandler<MvxValueEventArgs<bool>> ViewWillDisappearCalled;
    }
}
