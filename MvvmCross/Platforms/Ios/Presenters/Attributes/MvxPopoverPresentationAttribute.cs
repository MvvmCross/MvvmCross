// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Ios.Presenters.Attributes;

public class MvxPopoverPresentationAttribute : MvxBasePresentationAttribute
{
    public static readonly bool DefaultWrapInNavigationController = false;
    public static readonly CGSize DefaultPreferredContentSize = CGSize.Empty;
    public static readonly bool DefaultAnimated = true;
    public static readonly UIPopoverArrowDirection DefaultPermittedArrowDirections = UIPopoverArrowDirection.Any;

    public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

    public CGSize PreferredContentSize { get; set; } = DefaultPreferredContentSize;

    public bool Animated { get; set; } = DefaultAnimated;

    public UIPopoverArrowDirection PermittedArrowDirections { get; set; } = DefaultPermittedArrowDirections;
}
