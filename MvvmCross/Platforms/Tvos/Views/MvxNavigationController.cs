// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

using Foundation;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    public class MvxNavigationController : UINavigationController
    {
        public MvxNavigationController()
        {
        }

        public MvxNavigationController(NSCoder coder) : base(coder)
        {
        }

        public MvxNavigationController(UIViewController rootViewController) : base(rootViewController)
        {
        }

        public MvxNavigationController(Type navigationBarType, Type toolbarType) : base(navigationBarType, toolbarType)
        {
        }

        public MvxNavigationController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected MvxNavigationController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxNavigationController(IntPtr handle) : base(handle)
        {
        }
    }
}
