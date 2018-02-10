﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Plugin.Sidebar.Views
{
    /// <summary>
    /// A wrapper UIViewController to present a UISplitViewController view nested
    /// in a navigation stack instead of being the root application view controller
    /// class
    /// </summary>
    /// <seealso cref="UIKit.UIViewController" />
    [MvxSidebarPresentation(MvxPanelEnum.Center, MvxPanelHintType.PushPanel, true)]
    public class MvxSplitViewControllerHost : UIViewController
    {
        /// <summary>
        /// Displays the content UISplitViewController.
        /// </summary>
        /// <param name="content">The content.</param>
        public void DisplayContentController(UISplitViewController content)
        {
            AddChildViewController(content);
            content.View.Frame = View.Frame;
            View.AddSubview(content.View);
            DidMoveToParentViewController(this);
        }
    }
}
