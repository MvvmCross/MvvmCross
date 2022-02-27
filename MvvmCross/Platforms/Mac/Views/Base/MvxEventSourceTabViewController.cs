// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using Foundation;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Mac.Views.Base
{
    public class MvxEventSourceTabViewController
    : NSTabViewController, IMvxEventSourceViewController
    {
        protected MvxEventSourceTabViewController()
            : base()
        {
            this.Initialize();
        }

        protected MvxEventSourceTabViewController(NSCoder coder)
            : base(coder)
        {
            this.Initialize();
        }

        protected MvxEventSourceTabViewController(IntPtr handle)
            : base(handle)
        {
            this.Initialize();
        }

        protected MvxEventSourceTabViewController(NSObjectFlag flag)
            : base(flag)
        {
            this.Initialize();
        }

        private void Initialize()
        {
        }

        public override void LoadView()
        {
            base.LoadView();
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

        public override void DidSelect(NSTabView tabView, NSTabViewItem item)
        {
            base.DidSelect(tabView, item);
            DidSelectCalled?.Raise(this);
        }

        public override void WillSelect(NSTabView tabView, NSTabViewItem item)
        {
            base.WillSelect(tabView, item);
            WillSelectCalled?.Raise(this);
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

        public event EventHandler DidSelectCalled;

        public event EventHandler WillSelectCalled;

        public event EventHandler DisposeCalled;
    }
}
