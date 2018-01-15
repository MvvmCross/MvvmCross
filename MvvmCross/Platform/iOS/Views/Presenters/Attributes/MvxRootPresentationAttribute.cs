﻿using MvvmCross.Core.Views;
using UIKit;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxRootPresentationAttribute : MvxBasePresentationAttribute
    {
        public static float DefaultAnimationDuration = 1.0f;

        public static bool DefaultWrapInNavigationController = false;

        public static UIViewAnimationOptions DefaultAnimationOptions = UIViewAnimationOptions.TransitionNone;

        public float AnimationDuration { get; set; } = DefaultAnimationDuration;

        public UIViewAnimationOptions AnimationOptions { get; set; } = DefaultAnimationOptions;

        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
