// MvxEventSourceViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using System;

#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace Cirrious.CrossCore.Mac.Views
{
    public class MvxEventSourceViewController
        : NSViewController
          , IMvxEventSourceViewController
    {
        protected MvxEventSourceViewController()
        {
            Initialize();
        }

        protected MvxEventSourceViewController(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        protected MvxEventSourceViewController(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }

        protected MvxEventSourceViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            ViewDidLoad();
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

        public event EventHandler DisposeCalled;
    }
}