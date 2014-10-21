﻿// MvxEventSourceCollectionViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Foundation;
using UIKit;

namespace Cirrious.CrossCore.Touch.Views
{
    public class MvxEventSourceCollectionViewController
        : UICollectionViewController
          , IMvxEventSourceViewController
    {
        protected MvxEventSourceCollectionViewController(UICollectionViewLayout layout)
            : base(layout)
        {
        }

        protected MvxEventSourceCollectionViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxEventSourceCollectionViewController(string nibName, NSBundle bundle)
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
            ViewDidAppearCalled.Raise(this, animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewWillAppearCalled.Raise(this, animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewDidDisappearCalled.Raise(this, animated);
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
        public event EventHandler<MvxValueEventArgs<bool>> ViewWillAppearCalled;
        public event EventHandler<MvxValueEventArgs<bool>> ViewDidAppearCalled;
        public event EventHandler<MvxValueEventArgs<bool>> ViewDidDisappearCalled;
        public event EventHandler<MvxValueEventArgs<bool>> ViewWillDisappearCalled;
        public event EventHandler DisposeCalled;
    }
}