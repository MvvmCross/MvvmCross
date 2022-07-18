// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
#nullable enable
    public interface IMvxIosViewPresenter : IMvxViewPresenter, IMvxCanCreateIosView
    {
        public void ClosedPopoverViewController();

        public ConfiguredTaskAwaitable<bool> ClosedModalViewController(UIViewController viewController,
            MvxModalPresentationAttribute attribute);
    }
#nullable restore
}
