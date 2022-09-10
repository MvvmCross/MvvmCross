// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
#nullable enable
    public class MvxPopoverPresentationControllerDelegate : UIPopoverPresentationControllerDelegate
    {
        private readonly IMvxIosViewPresenter _presenter;

        public MvxPopoverPresentationControllerDelegate(IMvxIosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController)
        {
            return UIModalPresentationStyle.None;
        }

        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController controller, UITraitCollection traitCollection)
        {
            return UIModalPresentationStyle.None;
        }

        public override void DidDismissPopover(UIPopoverPresentationController popoverPresentationController)
        {
            _presenter.ClosedPopoverViewController();
        }
    }
#nullable restore
}
