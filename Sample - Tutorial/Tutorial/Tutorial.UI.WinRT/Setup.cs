using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WinRT.Platform;
using Tutorial.Core.ViewModels;
using Tutorial.Core.ViewModels.Lessons;
using Windows.UI.Xaml.Controls;

namespace Tutorial.UI.WinRT
{
    public class Setup
        : MvxBaseWinRTSetup
    {
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app = new Tutorial.Core.App();
            return app;
        }
    }
}
