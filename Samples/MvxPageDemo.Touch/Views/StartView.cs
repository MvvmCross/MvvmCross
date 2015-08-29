using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using MvxPageDemo.ViewModels;

namespace MvxPageDemo.Touch.Views
{
	public class StartView : MvxViewController<StartViewModel>
	{
		private UIButton _showB = null;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();
			CreateUI ();
			var set = this.CreateBindingSet<StartView, StartViewModel>();
			set.Bind (_showB).To (vm => vm.ShowCommand);
			set.Apply();
		}

		private void CreateUI()
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			Title = "StartView";
			View.BackgroundColor = UIColor.White;
			_showB = new UIButton (UIButtonType.System);
			_showB.TranslatesAutoresizingMaskIntoConstraints = false;
			_showB.SetTitle ("Show PagedController", UIControlState.Normal);
			View.AddSubview (_showB);
			View.AddConstraint (NSLayoutConstraint.Create (_showB, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterX, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (_showB, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterY, 1, 0));
		}
	}
}