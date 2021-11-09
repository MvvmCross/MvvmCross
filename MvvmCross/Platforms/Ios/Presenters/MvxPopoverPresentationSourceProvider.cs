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
        private readonly WeakReference<UIView?> _sourceView = new WeakReference<UIView?>(null);
        private readonly WeakReference<UIBarButtonItem?> _sourceBarButtonItem = new WeakReference<UIBarButtonItem?>(null);

        public UIView? SourceView
        {
            get
            {
                if (_sourceView.TryGetTarget(out var view))
                    return view;
                return null;
            }
            set
            {
                _sourceBarButtonItem.SetTarget(null);
                _sourceView.SetTarget(value);
            }
        }

        public UIBarButtonItem? SourceBarButtonItem
        {
            get
            {
                if (_sourceBarButtonItem.TryGetTarget(out var view))
                    return view;
                return null;
            }
            set
            {
                _sourceView.SetTarget(null);
                _sourceBarButtonItem.SetTarget(value);
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
