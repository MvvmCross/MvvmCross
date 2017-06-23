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
        : NSViewController, IMvxEventSourceViewController
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
            //this.ViewDidLoad();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewDidLoadCalled?.Raise(this);
        }

        public override void ViewDidLayout()
        {
            base.ViewDidLayout();
            ViewDidLayoutCalled?.Raise(this);
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            ViewWillAppearCalled?.Raise(this);
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
            ViewDidAppearCalled?.Raise(this);
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            ViewWillDisappearCalled?.Raise(this);
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
            ViewDidDisappearCalled?.Raise(this);
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

        public event EventHandler ViewDidLayoutCalled;

        public event EventHandler ViewWillAppearCalled;

        public event EventHandler ViewDidAppearCalled;

        public event EventHandler ViewDidDisappearCalled;

        public event EventHandler ViewWillDisappearCalled;

        public event EventHandler DisposeCalled;
    }
}