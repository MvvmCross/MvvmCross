using Cirrious.CrossCore.Core;
using System;
using UIKit;

namespace Cirrious.CrossCore.Touch.Views
{
    public class MvxEventSourcePageViewController : UIPageViewController, IMvxEventSourceViewController
    {
        public MvxEventSourcePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation orientation, UIPageViewControllerSpineLocation spine) : base(style, orientation, spine)
        {
        }

        public MvxEventSourcePageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewDidLoadCalled.Raise(this);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewWillAppearCalled.Raise(this, animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewDidAppearCalled.Raise(this, animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewDidDisappearCalled.Raise(this, animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewWillDisappearCalled.Raise(this, animated);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                DisposeCalled.Raise(this);
            base.Dispose(disposing);
        }

        public event EventHandler ViewDidLoadCalled;

        public event EventHandler<Cirrious.CrossCore.Core.MvxValueEventArgs<bool>> ViewWillAppearCalled;

        public event EventHandler<Cirrious.CrossCore.Core.MvxValueEventArgs<bool>> ViewDidAppearCalled;

        public event EventHandler<Cirrious.CrossCore.Core.MvxValueEventArgs<bool>> ViewDidDisappearCalled;

        public event EventHandler<Cirrious.CrossCore.Core.MvxValueEventArgs<bool>> ViewWillDisappearCalled;

        public event EventHandler DisposeCalled;
    }
}