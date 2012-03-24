using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Console.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using TwitterSearch.Core;
using TwitterSearch.Core.ViewModels;
using TwitterSearch.UI.Console.Views;

namespace TwitterSearch.UI.Console
{
    public class Setup
        : MvxBaseConsoleSetup
          , IMvxServiceProducer<IMvxStartNavigation>
    {
        protected override MvxApplication CreateApp()
        {
            var app = new TwitterSearchApp();
            return app;
        }
    }
}