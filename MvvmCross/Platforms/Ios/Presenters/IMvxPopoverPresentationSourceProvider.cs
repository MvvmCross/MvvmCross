// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public interface IMvxPopoverPresentationSourceProvider
    {
        UIView SourceView { get; set; }
        UIBarButtonItem SourceBarButtonItem { get; set; }
        public void SetSource(UIPopoverPresentationController popoverPresentationController);
    }
}
