﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Plugin.Sidebar.Views
{
    /// <summary>
    /// A derived UISplitViewController to make presentation of lots of
    /// data nicer in iPads and large screen devices.
    /// </summary>
    /// <seealso cref="UIKit.UISplitViewController" />
    public class MvxBaseSplitViewController : UISplitViewController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MvxBaseSplitViewController"/> class.
        /// </summary>
        public MvxBaseSplitViewController()
        {
            PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
            PreferredPrimaryColumnWidthFraction = 0.3f;
            ViewControllers = new[]
                {
                    new UIViewController(),
                    new UIViewController(),
                };
        }

        /// <summary>
        /// Sets the left (master) view controller.
        /// </summary>
        /// <param name="left">The left.</param>
        public void SetLeft(UIViewController left)
        {
            ViewControllers = new[]
                {
                    left,
                    ViewControllers[1]
                };
        }

        /// <summary>
        /// Sets the right (details) view controller.
        /// </summary>
        /// <param name="right">The right.</param>
        public void SetRight(UIViewController right)
        {
            ShowDetailViewController(right, null);
        }
    }
}
