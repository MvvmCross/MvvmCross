using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsStore.Platform;
using Tutorial.Core.ViewModels;
using Tutorial.Core.ViewModels.Lessons;
using Windows.UI.Xaml.Controls;

namespace Tutorial.UI.WinRT
{
    public class Setup
        : MvxStoreSetup
    {
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            var app = new Tutorial.Core.App();
            return app;
        }
    }
}
