// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Platforms.Ios.Presenters.Attributes;

namespace MvvmCross.Platforms.Ios.Presenters;

public sealed class MvxModalPresentationControllerDelegate(
        MvxIosViewPresenter presenter,
        UIViewController viewController,
        MvxModalPresentationAttribute attribute)
    : UIAdaptivePresentationControllerDelegate
{
    public override void DidDismiss(UIPresentationController presentationController)
    {
        _ = presenter.CloseModalViewController(viewController, attribute);
    }
}
