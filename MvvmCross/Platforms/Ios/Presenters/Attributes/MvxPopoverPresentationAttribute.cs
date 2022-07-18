// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using CoreGraphics;
using MvvmCross.Presenters.Attributes;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters.Attributes
{
    public class MvxPopoverPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

        public static CGSize DefaultPreferredContentSize = CGSize.Empty;
        public CGSize PreferredContentSize { get; set; } = DefaultPreferredContentSize;

        public static bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;

        public static UIPopoverArrowDirection DefaultPermittedArrowDirections = UIPopoverArrowDirection.Any;
        public UIPopoverArrowDirection PermittedArrowDirections { get; set; } = DefaultPermittedArrowDirections;
    }
}
