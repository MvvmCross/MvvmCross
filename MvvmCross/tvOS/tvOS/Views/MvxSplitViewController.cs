﻿using System;
using System.Linq;

using Foundation;
using UIKit;

using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.tvOS.Views
{
    public class MvxSplitViewController 
        : MvxEventSourceSplitViewController
        ,IMvxTvosView
    {
        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

        public MvxSplitViewController()
        {
            this.AdaptForBinding();
        }

        public MvxSplitViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        protected MvxSplitViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel?.ViewCreated();

            PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
            PreferredPrimaryColumnWidthFraction = .3f;
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
            {
                ViewModel?.ViewDestroy();
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxSplitViewController<TViewModel> : MvxSplitViewController, IMvxTvosView<TViewModel>
       where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxSplitViewController()
        {
        }

        public MvxSplitViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxSplitViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }
    }
}
