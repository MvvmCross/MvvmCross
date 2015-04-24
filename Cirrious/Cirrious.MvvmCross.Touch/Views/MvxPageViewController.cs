using System;
using UIKit;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using Cirrious.CrossCore.Touch.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
	public class MvxPageViewController : MvxEventSourcePageViewController, IMvxTouchView
	{
		private Dictionary<string,UIViewController> _pagedViewControllerCache = null;

		public MvxPageViewController (UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal,UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None) : base(style,orientation,spine)
		{
			this.AdaptForBinding ();
			_pagedViewControllerCache = new Dictionary<string, UIViewController> ();
		}

		public MvxPageViewController(IntPtr handle) : base(handle)
		{
			this.AdaptForBinding ();
			_pagedViewControllerCache = new Dictionary<string, UIViewController> ();
		}

		public MvxViewModelRequest Request { get; set; }
		public IMvxBindingContext BindingContext { get; set; }
		public IMvxViewModel ViewModel
		{
			get { return(DataContext as IMvxViewModel); }
			set {
				DataContext = value;
				//Verify ViewModel is IMvxPageViewModel
				if ((DataContext != null) && (!(DataContext is IMvxPageViewModel)))
					MvxTrace.Error ("Error - MvxPageViewController must be given an instance of IMvxPageViewModel");
			}
		}
		public object DataContext
		{
			get { return(BindingContext.DataContext); }
			set { BindingContext.DataContext = value; }
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			IMvxPageViewModel pageVM = ViewModel as IMvxPageViewModel;
			if (pageVM == null)
				return;
			IMvxPagedViewModel defaultVM = pageVM.GetDefaultViewModel ();
			UIViewController defaultVC = this.CreateViewControllerFor (defaultVM) as UIViewController;
			_pagedViewControllerCache [defaultVM.PagedViewId] = defaultVC;
			SetViewControllers (new UIViewController[] { defaultVC }, UIPageViewControllerNavigationDirection.Forward, true, null);
			GetNextViewController = delegate(UIPageViewController pc, UIViewController rc) {
				IMvxTouchView rcTV = rc as IMvxTouchView;
				if(rcTV == null)
					return(null);
				IMvxPagedViewModel currentVM = rcTV.ViewModel as IMvxPagedViewModel;
				if(currentVM == null)
					return(null);
				IMvxPagedViewModel nextVM = pageVM.GetNextViewModel (currentVM);
				if(nextVM == null)
					return(null);
				UIViewController nextVC = null;
				if(_pagedViewControllerCache.ContainsKey(nextVM.PagedViewId))
					nextVC = _pagedViewControllerCache[nextVM.PagedViewId];
				else {
					nextVC = this.CreateViewControllerFor(nextVM) as UIViewController;
					_pagedViewControllerCache[nextVM.PagedViewId] = nextVC;
				}
				return(nextVC);
			};
			GetPreviousViewController = delegate(UIPageViewController pc, UIViewController rc) {
				IMvxTouchView rcTV = rc as IMvxTouchView;
				if(rcTV == null)
					return(null);
				IMvxPagedViewModel currentVM = rcTV.ViewModel as IMvxPagedViewModel;
				if(currentVM == null)
					return(null);
				IMvxPagedViewModel prevVM = pageVM.GetPreviousViewModel (currentVM);
				if(prevVM == null)
					return(null);
				UIViewController prevVC = null;
				if(_pagedViewControllerCache.ContainsKey(prevVM.PagedViewId))
					prevVC = _pagedViewControllerCache[prevVM.PagedViewId];
				else {
					prevVC = this.CreateViewControllerFor(prevVM) as UIViewController;
					_pagedViewControllerCache[prevVM.PagedViewId] = prevVC;
				}
				return(prevVC);
			};
		}
	}

	public class MvxPageViewController<TViewModel> : MvxPageViewController, IMvxTouchView<TViewModel> where TViewModel : class, IMvxPageViewModel
	{
		public MvxPageViewController (UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal,UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None) : base(style,orientation,spine)
		{
		}

		public MvxPageViewController(IntPtr handle) : base(handle)
		{
		}

		public new TViewModel ViewModel
		{
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
	}
}
