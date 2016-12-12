using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Views.iOS;

using PageRendererExample.UI.iOS;

namespace pagerenderexample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate
    {
        private UIWindow _window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            var setup = new MvvmSetup(this, _window);
            setup.Initialize();

            _window.MakeKeyAndVisible();

            LoadApplication(setup.MvxFormsApp);

            return base.FinishedLaunching(app, options);
        }
    }
}

