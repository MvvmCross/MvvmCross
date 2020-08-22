// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public class MvxPopoverPresentationSourceProvider : IMvxPopoverPresentationSourceProvider
    {
        private UIView _sourceView;
        private UIBarButtonItem _sourceBarButtonItem;
        public UIView SourceView
        {
            get
            {
                var sourceView = _sourceView;
                _sourceView = null;
                return sourceView;
            }
            set => _sourceView = value;
        }
        public UIBarButtonItem SourceBarButtonItem
        {
            get
            {
                var sourceBarButtonItem = _sourceBarButtonItem;
                _sourceBarButtonItem = null;
                return sourceBarButtonItem;
            }
            set => _sourceBarButtonItem = value;
        }
        public void SetSource(UIPopoverPresentationController popoverPresentationController)
        {
            if (SourceView == null && SourceBarButtonItem == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(IMvxPopoverPresentationSourceProvider)} should contain a source for popover."
                );
            }
            if (SourceView != null)
            {
                popoverPresentationController.SourceView = SourceView;
                popoverPresentationController.SourceRect = SourceView.Frame;
            }
            else
            {
                popoverPresentationController.BarButtonItem = SourceBarButtonItem;
            }
        }
    }
}
