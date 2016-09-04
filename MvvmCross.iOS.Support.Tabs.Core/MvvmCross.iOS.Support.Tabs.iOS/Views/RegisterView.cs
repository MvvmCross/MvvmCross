using System;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Support.Tabs.Core.ViewModels;
using MvvmCross.iOS.Support.Presenters;
using UIKit;
using Cirrious.FluentLayouts.Touch;
namespace MvvmCross.iOS.Support.Tabs.iOS.Views
{
	[MvxTabPresentation(MvxTabPresentationMode.Child)]
	public class RegisterView : MvxViewController<RegisterViewModel>
	{
		private UILabel _lblSample;

		public RegisterView()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_lblSample = new UILabel
			{
				Text = "This view aims to show how to show a ViewController as a child outside TabBarController",
				Lines = 0,
				LineBreakMode = UILineBreakMode.WordWrap
			};

			View.AddSubviews(_lblSample);

			View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

			View.AddConstraints(
				_lblSample.AtLeftOf(View),
				_lblSample.AtRightOf(View),
				_lblSample.WithSameCenterY(View)
			);
		}
	}
}

