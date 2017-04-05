// MvxPageViewController.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.tvOS.Views;

    using UIKit;
    using Foundation;

	public class MvxPageViewController : MvxEventSourcePageViewController, IMvxTvosView
	{
		protected MvxPageViewController(
			UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,
			UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal,
			UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None)
			: base(style, orientation, spine)
		{
			this.AdaptForBinding();
		}

		public MvxPageViewController(IntPtr handle)
			: base(handle)
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

	public class MvxPageViewController<TViewModel> : MvxPageViewController, IMvxTvosView<TViewModel> where TViewModel : class, IMvxViewModel
	{

		protected MvxPageViewController(
			UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,
			UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal,
			UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None)
			: base(style, orientation, spine)
		{
		}

		public MvxPageViewController(IntPtr handle)
			: base(handle)
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

	}
}