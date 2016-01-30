namespace MvvmCross.Platform.iOS.Views
{
    using System;

    using MvvmCross.Platform.Core;

    using UIKit;

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
            this.ViewDidLoadCalled.Raise(this);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.ViewWillAppearCalled.Raise(this, animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.ViewDidAppearCalled.Raise(this, animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.ViewDidDisappearCalled.Raise(this, animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.ViewWillDisappearCalled.Raise(this, animated);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.DisposeCalled.Raise(this);
            base.Dispose(disposing);
        }

        public event EventHandler ViewDidLoadCalled;

        public event EventHandler<MvxValueEventArgs<bool>> ViewWillAppearCalled;

        public event EventHandler<MvxValueEventArgs<bool>> ViewDidAppearCalled;

        public event EventHandler<MvxValueEventArgs<bool>> ViewDidDisappearCalled;

        public event EventHandler<MvxValueEventArgs<bool>> ViewWillDisappearCalled;

        public event EventHandler DisposeCalled;
    }
}