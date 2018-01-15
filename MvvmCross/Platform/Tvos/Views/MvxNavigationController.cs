using System;

using Foundation;
using UIKit;

namespace MvvmCross.tvOS.Views
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
    }
}
