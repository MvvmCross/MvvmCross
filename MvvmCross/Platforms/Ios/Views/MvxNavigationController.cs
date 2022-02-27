// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Foundation;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    public class MvxNavigationController : UINavigationController
    {
        public MvxNavigationController()
        {
        }

        public MvxNavigationController(UIViewController rootViewController) : base(rootViewController)
        {
        }

        public MvxNavigationController(NSCoder coder) : base(coder)
        {
        }

        public MvxNavigationController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public MvxNavigationController(Type navigationBarType, Type toolbarType) : base(navigationBarType, toolbarType)
        {
        }

        protected MvxNavigationController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxNavigationController(IntPtr handle) : base(handle)
        {
        }

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return TopViewController?.GetSupportedInterfaceOrientations() ?? base.GetSupportedInterfaceOrientations();
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            return TopViewController?.PreferredInterfaceOrientationForPresentation() ?? base.PreferredInterfaceOrientationForPresentation();
        }

        public override bool ShouldAutorotate()
        {
            return TopViewController?.ShouldAutorotate() ?? base.ShouldAutorotate();
        }
    }
}
