using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Mac.Platform;
using MvvmCross.Mac.Views.Presenters;
using MvvmCross.Mac.Views.Presenters.Attributes;
using Playground.Core;

namespace Playground.Mac
{
    public class Setup : MvxMacSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate) : base(applicationDelegate)
        {
            MvxWindowPresentationAttribute.DefaultWidth = 250;
            MvxWindowPresentationAttribute.DefaultHeight = 250;
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }
    }
}
