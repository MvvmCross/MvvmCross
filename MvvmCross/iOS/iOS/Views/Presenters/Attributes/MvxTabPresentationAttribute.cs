﻿using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxTabPresentationAttribute : MvxBasePresentationAttribute
    {
        public string TabName { get; set; }

        public string TabIconName { get; set; }

        public string TabSelectedIconName { get; set; }

        public bool WrapInNavigationController { get; set; } = true;

        public string TabAccessibilityIdentifier { get; set; }
    }
}
