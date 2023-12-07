// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platforms.Ios.Presenters.Attributes;

namespace MvvmCross.Platforms.Ios.Presenters;

public class MvxModalPresentationControllerDelegate : UIAdaptivePresentationControllerDelegate
{
    private readonly MvxIosViewPresenter _presenter;
    private readonly UIViewController _viewController;
    private readonly MvxModalPresentationAttribute _attribute;

    public MvxModalPresentationControllerDelegate(MvxIosViewPresenter presenter, UIViewController viewController, MvxModalPresentationAttribute attribute)
    {
        _presenter = presenter;
        _viewController = viewController;
        _attribute = attribute;
    }

    public override async void DidDismiss(UIPresentationController presentationController)
    {
        await _presenter.CloseModalViewController(_viewController, _attribute).ConfigureAwait(false);
    }
}