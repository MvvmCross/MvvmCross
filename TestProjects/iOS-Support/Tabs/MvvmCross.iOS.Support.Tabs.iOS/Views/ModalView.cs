using System;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Support.Tabs.Core.ViewModels;
using MvvmCross.iOS.Support.Presenters;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;

namespace MvvmCross.iOS.Support.Tabs.iOS.Views
{
	[MvxTabPresentation(MvxTabPresentationMode.Modal, true)]
	public class ModalView : MvxViewController<ModalViewModel>
	{
		private UILabel _lblSample;

		private UIButton _btnClose;

		public ModalView()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_lblSample = new UILabel
			{
				Text = "This is a modal view wrapped in a NavigationController!",
				Lines = 0,
				LineBreakMode = UILineBreakMode.WordWrap
			};

			_btnClose = new UIButton();
			_btnClose.SetTitle("Close me", UIControlState.Normal);
			_btnClose.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);

			View.AddSubviews(_lblSample, _btnClose);

			View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

			View.AddConstraints(
				_lblSample.WithSameCenterY(View),
				_lblSample.AtLeftOf(View, 8f),
				_lblSample.AtRightOf(View, 8f),

				_btnClose.Below(_lblSample, 16f),
				_btnClose.WithSameCenterX(View)
			);

			var set = this.CreateBindingSet<ModalView, ModalViewModel>();
			set.Bind(_btnClose).To(vm => vm.CloseCommand);
			set.Apply();
		}
	}
}

