using System;
using Cirrious.MvvmCross.Touch.Views;
using MvxPageDemo.ViewModels;
using UIKit;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace MvxPage.iOS.Views
{
	public class ThirdPageView : MvxViewController<ThirdPageViewModel>
	{
		private UILabel _pageL = null;

		public ThirdPageView()
		{
			Console.WriteLine ("Constructing ThirdPageView");
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();
			CreateUI ();
			var set = this.CreateBindingSet<ThirdPageView, ThirdPageViewModel>();
			set.Bind (this).For(s => s.Title).To (vm => vm.PageTitle);
			set.Bind (_pageL).To (vm => vm.PageTitle);
			set.Apply();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.ParentViewController.Title = Title;
		}

		private void CreateUI()
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			View.BackgroundColor = UIColor.Blue;
			_pageL = new UILabel ();
			_pageL.TranslatesAutoresizingMaskIntoConstraints = false;
			View.AddSubview (_pageL);
			View.AddConstraint (NSLayoutConstraint.Create (_pageL, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterX, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (_pageL, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterY, 1, 0));
		}
	}
}
