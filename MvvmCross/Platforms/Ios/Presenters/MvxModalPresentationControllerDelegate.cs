// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platforms.Ios.Presenters.Attributes;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public class MvxModalPresentationControllerDelegate : UIAdaptivePresentationControllerDelegate
    {
        private readonly IMvxIosViewPresenter _presenter;
        private readonly UIViewController _viewController;
        private readonly MvxModalPresentationAttribute _attribute;

        public MvxModalPresentationControllerDelegate(IMvxIosViewPresenter presenter, UIViewController viewController, MvxModalPresentationAttribute attribute)
        {
            _presenter = presenter;
            _viewController = viewController;
            _attribute = attribute;
        }

        public override void DidDismiss(UIPresentationController presentationController)
        {
            _presenter.ClosedModalViewController(_viewController, _attribute);
        }
    }
}
