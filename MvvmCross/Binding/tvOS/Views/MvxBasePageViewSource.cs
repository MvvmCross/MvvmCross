// MvxBasePageViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.tvOS.Views
{
	using System;

	using MvvmCross.Platform;
	using MvvmCross.Platform.Exceptions;
	using UIKit;

	public abstract class MvxBasePageViewSource : UIPageViewControllerDataSource
	{
		private readonly UIPageViewController _pageView;

		public UIPageViewController PageView => this._pageView;

		protected MvxBasePageViewSource(UIPageViewController pageView)
		{
			this._pageView = pageView;
		}

		protected MvxBasePageViewSource(IntPtr handle)
            : base(handle)
        {
			Mvx.Warning("MvxBasePageViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
		}
			    
		public virtual void ReloadData()
		{
			try
			{
				var controller = GetInitialViewController();
				var list = new UIViewController[] { controller };
				SetViewControllers(list);

			}
			catch (Exception exception)
			{
				Mvx.Warning("Exception masked during PageView SetViewControllers {0}", exception.ToLongString());
			}
		}

		protected virtual void SetViewControllers(UIViewController[] viewControllers)
		{
			this._pageView.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
		}

		protected abstract UIViewController GetViewControllerAtIndex(int index);

		protected virtual UIViewController GetInitialViewController()
		{
			return GetViewControllerAtIndex(0);
		}
	}
}
