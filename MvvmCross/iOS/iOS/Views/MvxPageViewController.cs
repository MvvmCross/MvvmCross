namespace MvvmCross.iOS.Views
{
    using Binding.BindingContext;
    using Core.ViewModels;
    using Foundation;
    using MvvmCross.Platform.iOS.Views;
    using System;
    using UIKit;

    public class MvxPageViewController : MvxEventSourcePageViewController, IMvxIosView
    {
        protected MvxPageViewController(
            UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,
            UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal,
            UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None)
            : base(style, orientation, spine)
        {

            this.AdaptForBinding();
        }

        public MvxPageViewController(IntPtr handle) : base(handle)
        {
            this.AdaptForBinding();
        }
		public MvxPageViewController(string nibName, NSBundle bundle)
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
            get { return (this.DataContext as IMvxViewModel); }
            set { this.DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }

    public class MvxPageViewController<TViewModel> : MvxPageViewController, IMvxIosView<TViewModel> where TViewModel : class, IMvxViewModel
    {

        protected MvxPageViewController(
            UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,
            UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal,
            UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None)
            : base(style, orientation, spine)
        {
        }

        public MvxPageViewController(IntPtr handle) : base(handle)
        {
        }

		public MvxPageViewController(string nibName, NSBundle bundle)
			: base(nibName, bundle)
        {
		}

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }

    }
}
