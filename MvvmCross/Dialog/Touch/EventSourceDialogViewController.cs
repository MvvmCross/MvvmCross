// EventSourceDialogViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Touch
{
    using System;

    using CrossUI.Touch.Dialog;
    using CrossUI.Touch.Dialog.Elements;

    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Touch.Views;

    using UIKit;

    public class EventSourceDialogViewController
        : DialogViewController
          , IMvxEventSourceViewController
    {
        protected EventSourceDialogViewController(UITableViewStyle style = UITableViewStyle.Grouped,
                                                  RootElement root = null,
                                                  bool pushing = false)
            : base(style, root, pushing)
        {
        }

        public EventSourceDialogViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.ViewWillDisappearCalled.Raise(this, animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.ViewDidAppearCalled.Raise(this, animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.ViewWillAppearCalled.Raise(this, animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.ViewDidDisappearCalled.Raise(this, animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
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

        public event EventHandler<MvxValueEventArgs<bool>> ViewWillAppearCalled;

        public event EventHandler<MvxValueEventArgs<bool>> ViewDidAppearCalled;

        public event EventHandler<MvxValueEventArgs<bool>> ViewDidDisappearCalled;

        public event EventHandler<MvxValueEventArgs<bool>> ViewWillDisappearCalled;

        public event EventHandler DisposeCalled;
    }
}