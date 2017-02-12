// MvxViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System;

    using Foundation;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.iOS.Views;
    using UIKit;

    public class MvxViewController
        : MvxEventSourceViewController
          , IMvxIosView
    {
        public MvxViewController()
        {
            this.AdaptForBinding();
        }

        public MvxViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        protected MvxViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return this.DataContext as IMvxViewModel; }
            set { this.DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ViewModel?.Appearing();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			ViewModel?.Appeared();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			ViewModel?.Disappearing();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			ViewModel?.Disappeared();
		}

		public override void DidMoveToParentViewController (UIViewController parent)
		{
			base.DidMoveToParentViewController (parent);
			if (parent == null) {
				ViewModel?.Destroy ();
			}
		}

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender) {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxViewController<TViewModel>
        : MvxViewController
          , IMvxIosView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxViewController()
        {
        }

        public MvxViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}