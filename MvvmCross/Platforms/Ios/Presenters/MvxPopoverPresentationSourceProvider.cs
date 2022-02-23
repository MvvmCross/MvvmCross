// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
#nullable enable
    public class MvxPopoverPresentationSourceProvider : IMvxPopoverPresentationSourceProvider
    {
        [Weak] private UIView? _sourceView;
        [Weak] private UIBarButtonItem? _sourceBarButtonItem;

        public UIView? SourceView
        {
            get => _sourceView;
            set
            {
                _sourceBarButtonItem = null;
                _sourceView = value;
            }
        }

        public UIBarButtonItem? SourceBarButtonItem
        {
            get => _sourceBarButtonItem;
            set
            {
                _sourceView = null;
                _sourceBarButtonItem = value;
            }
        }

        public void SetSource(UIPopoverPresentationController popoverPresentationController)
        {
            if (popoverPresentationController == null)
                throw new ArgumentNullException(nameof(popoverPresentationController));

            if (SourceView == null && SourceBarButtonItem == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(IMvxPopoverPresentationSourceProvider)} should contain a source for popover."
                );
            }

            if (SourceView != null)
            {
                popoverPresentationController.SourceView = SourceView;
                popoverPresentationController.SourceRect = SourceView.Bounds;
                SourceView = null;
            }
            else
            {
                popoverPresentationController.BarButtonItem = SourceBarButtonItem;
                SourceBarButtonItem = null;
            }
        }
    }
#nullable restore
}
