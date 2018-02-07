// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using AppKit;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Mac.Views;

namespace MvvmCross.Platform.Mac.Views
{
    public class MvxTabViewController : MvxEventSourceTabViewController, IMvxTabViewController, IMvxMacView
    {
        protected MvxTabViewController()
            : base()
        {
            this.Initialize();
        }

        protected MvxTabViewController(NSCoder coder)
            : base(coder)
        {
            this.Initialize();
        }

        protected MvxTabViewController(IntPtr handle)
            : base(handle)
        {
            this.Initialize();
        }

        protected MvxTabViewController(NSObjectFlag flag)
            : base(flag)
        {
            this.Initialize();
        }

        // Shared initialization code
        private void Initialize()
        {
            this.AdaptForBinding();
        }

        public void ShowTabView(NSViewController viewController, string tabTitle)
        {
            AddChildViewController(viewController);

            if (!string.IsNullOrEmpty(tabTitle))
                TabViewItems[ChildViewControllers.Count() - 1].Label = tabTitle;
        }

        public bool CloseTabView(IMvxViewModel viewModel)
        {
            var index = ChildViewControllers.Select(v => (MvxViewController)v).ToList().FindIndex(vc => viewModel == vc.ViewModel);

            if (index >= 0)
            {
                RemoveChildViewController(index);
                return true;
            }

            return false;
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel?.ViewCreated();
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            ViewModel?.ViewAppearing();
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
            ViewModel?.ViewAppeared();
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            ViewModel?.ViewDisappearing();
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
            ViewModel?.ViewDisappeared();
        }

        public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxTabViewController<TViewModel>
        : MvxTabViewController, IMvxMacView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxTabViewController()
        {
        }

        public MvxTabViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxTabViewController(NSObjectFlag flag)
            : base(flag)
        {
        }

        public MvxTabViewController(NSCoder coder) : base(coder)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
