// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using CoreGraphics;
using MvvmCross.Presenters.Attributes;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Presenters.Attributes
{
    public class MvxModalPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

        public static UIModalPresentationStyle DefaultModalPresentationStyle = UIModalPresentationStyle.FullScreen;
        public UIModalPresentationStyle ModalPresentationStyle { get; set; } = DefaultModalPresentationStyle;

        public static UIModalTransitionStyle DefaultModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
        public UIModalTransitionStyle ModalTransitionStyle { get; set; } = DefaultModalTransitionStyle;

        public static CGSize DefaultPreferredContentSize = CGSize.Empty;
        public CGSize PreferredContentSize { get; set; } = DefaultPreferredContentSize;

        public static bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
    }
}
