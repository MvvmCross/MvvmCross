﻿using System;
using Cirrious.MvvmCross.Droid.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.CrossCore.Touch.Views
{
    public class EventSourceViewController
        : UIViewController
          , IViewControllerEventSource
    {
        protected EventSourceViewController()
        {
        }

        protected EventSourceViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewWillDisappearCalled.Raise(this, animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewDidDisappearCalled.Raise(this, animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewWillAppearCalled.Raise(this, animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewDidAppearCalled.Raise(this, animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewDidLoadCalled.Raise(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler ViewDidLoadCalled;
        public event EventHandler<TypedEventArgs<bool>> ViewWillAppearCalled;
        public event EventHandler<TypedEventArgs<bool>> ViewDidAppearCalled;
        public event EventHandler<TypedEventArgs<bool>> ViewDidDisappearCalled;
        public event EventHandler<TypedEventArgs<bool>> ViewWillDisappearCalled;
        public event EventHandler DisposeCalled;
    }
}