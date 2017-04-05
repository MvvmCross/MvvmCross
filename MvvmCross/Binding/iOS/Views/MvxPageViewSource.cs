// MvxPageViewSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;

	using MvvmCross.Binding.Attributes;
	using MvvmCross.Binding.ExtensionMethods;
	using MvvmCross.Binding.iOS.ViewModels;
	using MvvmCross.Core.Views;
	using MvvmCross.Platform;
	using MvvmCross.Platform.WeakSubscription;
	using UIKit;

	public abstract class MvxPageViewSource : MvxBasePageViewSource
	{
		private IEnumerable _itemSource;
		private IDisposable _subscription;

		protected MvxPageViewSource(UIPageViewController pageView)
			: base(pageView)
		{
		}

		protected MvxPageViewSource(IntPtr handle)
            : base(handle)
        {
			Mvx.Warning("TableViewSource IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_subscription != null)
				{
					_subscription.Dispose();
					_subscription = null;
				}
			}

			base.Dispose(disposing);
		}

		[MvxSetToNullAfterBinding]
		public virtual IEnumerable ItemSource
		{
			get { return _itemSource; }
			set
			{
				if (Object.ReferenceEquals(_itemSource, value))
					return;

				if (_subscription != null)
				{
					_subscription.Dispose();
					_subscription = null;
				}

				_itemSource = value;

				var collectionChanged = _itemSource as INotifyCollectionChanged;
				if (collectionChanged != null)
				{
					_subscription = collectionChanged.WeakSubscribe(CollectionChangedOnCollectionChanged);
				}

				ReloadData();
			}
		}

		public virtual bool CanLoop { get; set; }

		protected virtual void CollectionChangedOnCollectionChanged(object sender,
																	NotifyCollectionChangedEventArgs args)
		{
			ReloadData();
		}

		public virtual int GetPageIndexForController(UIViewController referenceViewController) {
			var mvxView = referenceViewController as IMvxView;
			if (mvxView != null)
			{
				var vm = mvxView.ViewModel as IMvxPageViewModel;
				if (vm != null)
					return vm.PageIndex;
			}

			var mvxPageView = referenceViewController as IMvxPageViewController;
			if (mvxPageView != null)
				return mvxPageView.PageIndex;

			return -1;
		}

		public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
		{
			if (ItemSource.Count() == 1)
				return null;

			var page = GetPageIndexForController(referenceViewController) + 1;
			if (page != 0)
			{
				var index = (page == ItemSource.Count()) ?
					(CanLoop ? 0 : -1) :
					(page);

				return index == -1 ? null : GetViewControllerAtIndex(index);
			}

			return null;
		}

		public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController) {
			if (ItemSource.Count() == 1)
				return null;

			var page = GetPageIndexForController(referenceViewController);
			if (page != -1)
			{
				var index = (page == 0) ?
					(CanLoop ? ItemSource.Count() : 0) - 1 :
					(page - 1);

				return index == -1 ? null : GetViewControllerAtIndex(index);
			}

			return null;
		}

		public override nint GetPresentationCount(UIPageViewController pageViewController)
		{
			var count = ItemSource.Count();
			return count == 1 ? 0 : count;
		}

		public override nint GetPresentationIndex(UIPageViewController pageViewController)
		{
			return 0;
		}
	}
}
