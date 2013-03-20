using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Console.Platform;
using Cirrious.MvvmCross.ViewModels;
using TwitterSearch.Core;
using TwitterSearch.Core.ViewModels;
using TwitterSearch.UI.Console.Views;

namespace TwitterSearch.UI.Console
{
    public class Setup
        : MvxConsoleSetup
    {
        protected override IMvxApplication CreateApp()
        {
            var app = new TwitterSearchApp();
            return app;
        }
    }
}