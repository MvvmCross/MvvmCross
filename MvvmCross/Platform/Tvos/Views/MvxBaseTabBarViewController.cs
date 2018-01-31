// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.tvOS.Views;

using Foundation;
using UIKit;

namespace MvvmCross.tvOS.Views
{
    public class MvxBaseTabBarViewController
        : MvxEventSourceTabBarController, IMvxTvosView 
    {

        public MvxBaseTabBarViewController()
        {
            this.AdaptForBinding();
        }

        protected MvxBaseTabBarViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get
            {
                // special code needed in TabBar because View is initialized during construction
                return BindingContext?.DataContext;
            }
            set
            {
                BindingContext.DataContext = value;
            }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
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

    public class MvxBaseTabBarViewController<TViewModel> : MvxBaseTabBarViewController, IMvxTvosView<TViewModel>
      where TViewModel : class, IMvxViewModel
    {
        protected MvxBaseTabBarViewController()
        {
        }

        protected MvxBaseTabBarViewController(IntPtr handle)
            : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
