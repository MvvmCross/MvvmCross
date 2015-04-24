using System;
using Cirrious.MvvmCross.Touch.Views;
using MvxPageDemo.ViewModels;
using UIKit;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace MvxPage.iOS.Views
{
	public class FirstPageView : MvxViewController<FirstPageViewModel>
	{
		private UILabel _pageL = null;

		public FirstPageView()
		{
			Console.WriteLine ("Constructing FirstPageView");
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();
			CreateUI ();
			var set = this.CreateBindingSet<FirstPageView, FirstPageViewModel>();
			set.Bind (this).For(s => s.Title).To (vm => vm.PageTitle);
			set.Bind (_pageL).To (vm => vm.PageTitle);
			set.Apply();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.ParentViewController.Title = Title;
		}

		private void CreateUI()
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			View.BackgroundColor = UIColor.Red;
			_pageL = new UILabel ();
			_pageL.TranslatesAutoresizingMaskIntoConstraints = false;
			View.AddSubview (_pageL);
			View.AddConstraint (NSLayoutConstraint.Create (_pageL, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterX, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (_pageL, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterY, 1, 0));
		}
	}
}
