using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using RoutingExample.Core;
using UIKit;

namespace RoutingExample.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window) { }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        
        protected override void InitializePlatformServices()
        {
            base.InitializePlatformServices();
        }
    }
}
