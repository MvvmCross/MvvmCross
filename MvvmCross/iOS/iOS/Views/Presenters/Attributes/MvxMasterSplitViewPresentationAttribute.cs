﻿using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxMasterSplitViewPresentationAttribute : MvxBasePresentationAttribute
    {
        public bool WrapInNavigationController { get; set; } = true;
    }
}
