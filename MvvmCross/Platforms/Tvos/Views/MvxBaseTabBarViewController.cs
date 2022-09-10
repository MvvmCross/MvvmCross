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
    public class MvxBaseTabBarViewController
        : MvxEventSourceTabBarController, IMvxTvosView
    {
        public MvxBaseTabBarViewController()
        {
            this.AdaptForBinding();
        }

        public MvxBaseTabBarViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected MvxBaseTabBarViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal MvxBaseTabBarViewController(IntPtr handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public MvxBaseTabBarViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
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
        public MvxBaseTabBarViewController()
        {
        }

        public MvxBaseTabBarViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxBaseTabBarViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected MvxBaseTabBarViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxBaseTabBarViewController(IntPtr handle) : base(handle)
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
