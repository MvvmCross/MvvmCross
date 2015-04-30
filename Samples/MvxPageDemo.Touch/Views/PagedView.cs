using System;
using Cirrious.MvvmCross.Touch.Views;
using MvxPageDemo.ViewModels;
using UIKit;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace MvxPage.iOS.Views
{
	public class PagedView : MvxPageViewController<PagedViewModel>,IMvxModalTouchView
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();
			CreateUI ();
			var set = this.CreateBindingSet<PagedView, PagedViewModel>();
			set.Apply();
		}

		private void CreateUI()
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			View.BackgroundColor = UIColor.Gray;
		}
	}
}
