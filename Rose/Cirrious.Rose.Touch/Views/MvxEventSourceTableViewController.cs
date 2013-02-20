﻿using System;
using Cirrious.CrossCore.Interfaces.Core;
using MonoTouch.UIKit;

namespace Cirrious.CrossCore.Touch.Views
{
    public class MvxEventSourceTableViewController
        : UITableViewController
          , IMvxEventSourceViewController
    {
        protected MvxEventSourceTableViewController(UITableViewStyle style = UITableViewStyle.Plain)
            : base(style)
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
        public event EventHandler<MvxTypedEventArgs<bool>> ViewWillAppearCalled;
        public event EventHandler<MvxTypedEventArgs<bool>> ViewDidAppearCalled;
        public event EventHandler<MvxTypedEventArgs<bool>> ViewDidDisappearCalled;
        public event EventHandler<MvxTypedEventArgs<bool>> ViewWillDisappearCalled;
        public event EventHandler DisposeCalled;
    }
}