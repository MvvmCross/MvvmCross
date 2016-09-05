using System;
using MvvmCross.iOS.Support.Presenters;
using MvvmCross.iOS.Support.Tabs.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;

namespace MvvmCross.iOS.Support.Tabs.iOS.Views
{
	[MvxTabPresentation(MvxTabPresentationMode.Tab, true)]
	public class Tab1View : MvxViewController<Tab1ViewModel>
	{
		private UIButton _btnChild, _btnModal;

		public Tab1View()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_btnModal = new UIButton();
			_btnModal.SetTitle("Open modal view", UIControlState.Normal);
			_btnModal.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);

			_btnChild = new UIButton();
			_btnChild.SetTitle("Open child view", UIControlState.Normal);
			_btnChild.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);

			View.AddSubviews(_btnChild, _btnModal);
			View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

			View.AddConstraints(
				_btnChild.WithSameCenterX(View),
				_btnChild.WithSameCenterY(View),

				_btnModal.Below(_btnChild, 8f),
				_btnModal.WithSameCenterX(View)
			);

			var set = this.CreateBindingSet<Tab1View, Tab1ViewModel>();
			set.Bind(_btnChild).To(vm => vm.OpenChildCommand);
			set.Bind(_btnModal).To(vm => vm.OpenModalCommand);
			set.Apply();
		}
	}
}

