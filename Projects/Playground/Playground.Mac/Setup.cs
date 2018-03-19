﻿using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core;

namespace Playground.Mac
{
    public class Setup : MvxMacSetup<App>
    {
        public Setup()
        {
            MvxWindowPresentationAttribute.DefaultWidth = 250;
            MvxWindowPresentationAttribute.DefaultHeight = 250;
        }
    }
}
