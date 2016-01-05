// MvxEventSourceViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


namespace MvvmCross.Platform.Mac.Views
{
    using System;

    using AppKit;
    using Foundation;

    using MvvmCross.Platform.Core;

    public class MvxEventSourceViewController
        : NSViewController
          , IMvxEventSourceViewController
    {
        protected MvxEventSourceViewController()
        {
            this.Initialize();
        }

        protected MvxEventSourceViewController(IntPtr handle)
            : base(handle)
        {
            this.Initialize();
        }

        protected MvxEventSourceViewController(NSCoder coder)
            : base(coder)
        {
            this.Initialize();
        }

        protected MvxEventSourceViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.Initialize();
        }

        private void Initialize()
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            this.ViewDidLoad();
        }

        //        public override void ViewWillDisappear(bool animated)
        //		{
        //            base.ViewWillDisappear(animated);
        //            ViewWillDisappearCalled.Raise(this, animated);
        //        }
        //
        //        public override void ViewDidAppear(bool animated)
        //        {
        //            base.ViewDidAppear(animated);
        //            ViewDidDisappearCalled.Raise(this, animated);
        //        }
        //
        //        public override void ViewWillAppear(bool animated)
        //        {
        //            base.ViewWillAppear(animated);
        //            ViewWillAppearCalled.Raise(this, animated);
        //        }
        //
        //        public override void ViewDidDisappear(bool animated)
        //        {
        //            base.ViewDidDisappear(animated);
        //            ViewDidAppearCalled.Raise(this, animated);
        //        }
        //
        public virtual void ViewDidLoad()
        {
            this.ViewDidLoadCalled.Raise(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler ViewDidLoadCalled;

        public event EventHandler DisposeCalled;
    }
}