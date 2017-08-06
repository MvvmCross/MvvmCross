﻿using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxChildPresentationAttribute : MvxBasePresentationAttribute
    {
        public bool Animated { get; set; } = true;
    }
}
