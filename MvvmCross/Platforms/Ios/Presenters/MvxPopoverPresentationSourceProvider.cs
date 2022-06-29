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
        private readonly WeakReference<UIView?> _sourceViewWeakReference = new WeakReference<UIView?>(null);
        private readonly WeakReference<UIBarButtonItem?> _sourceBarButtonItemWeakReference = new WeakReference<UIBarButtonItem?>(null);

        public UIView? SourceView
        {
            get
            {
                if (_sourceViewWeakReference.TryGetTarget(out var view))
                    return view;

                // This is not a array Sonar. You are drunk...
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }
            set
            {
                _sourceBarButtonItemWeakReference.SetTarget(null);
                _sourceViewWeakReference.SetTarget(value);
            }
        }

        public UIBarButtonItem? SourceBarButtonItem
        {
            get
            {
                if (_sourceBarButtonItemWeakReference.TryGetTarget(out var view))
                    return view;
                return null;
            }
            set
            {
                _sourceViewWeakReference.SetTarget(null);
                _sourceBarButtonItemWeakReference.SetTarget(value);
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
