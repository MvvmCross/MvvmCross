// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Tvos.Views.Base;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    public class MvxTableViewController
        : MvxEventSourceTableViewController, IMvxTvosView
    {
        protected MvxTableViewController(UITableViewStyle style = UITableViewStyle.Plain)
            : base(style)
        {
            this.AdaptForBinding();
        }

        protected MvxTableViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        protected MvxTableViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get
            {
                /*
				MvxLog.Instance.Trace ("I am in .ViewModel!");
				if (BindingContext == null)
					MvxLog.Instance.Trace ("BindingContext is null!");
				MvxLog.Instance.Trace ("I am in .ViewModel 2!");
				if (DataContext == null)
					MvxLog.Instance.Trace ("DataContext is null!");
				MvxLog.Instance.Trace ("I am in .ViewModel 3!");

				var c = DataContext;
				MvxLog.Instance.Trace ("I am in .ViewModel 4!");
				var d = c as IMvxViewModel;
				MvxLog.Instance.Trace ("I am in .ViewModel 5!");

				var e = (IMvxViewModel)d;
				MvxLog.Instance.Trace ("I am in .ViewModel 6!");
				if (d == null)
					MvxLog.Instance.Trace ("d was null!");

				if (e == null)
					MvxLog.Instance.Trace ("e was null!");
				*/
                return DataContext as IMvxViewModel;
            }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel?.ViewCreated();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewModel?.ViewAppearing();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewModel?.ViewAppeared();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewModel?.ViewDisappearing();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewModel?.ViewDisappeared();
        }

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            if (parent == null)
                ViewModel?.ViewDestroy();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxTableViewController<TViewModel> : MvxTableViewController, IMvxTvosView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxTableViewController(UITableViewStyle style = UITableViewStyle.Plain)
            : base(style)
        {
        }

        protected MvxTableViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxTableViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxTvosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxTvosView<TViewModel>, TViewModel>();
        }
    }
}
