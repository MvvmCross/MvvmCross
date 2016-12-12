using System;
using MvvmCross.iOS.Views;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.iOS.Support.Presenters;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Support.Tabs.Core.ViewModels;

namespace MvvmCross.iOS.Support.Tabs.iOS.Views
{
	[MvxTabPresentation(MvxTabPresentationMode.Root)]
	public class LoginView : MvxViewController<LoginViewModel>
	{
		private UITextField _txtUsername, _txtPassword;

		private UIButton _btnLogin, _btnRegister, _btnModal;

		private UIView _viewContainer;

		public LoginView()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ClearNavigationStack();

			_txtUsername = new UITextField
			{
				Placeholder = "Username",
				BorderStyle = UITextBorderStyle.RoundedRect,
				AutocapitalizationType = UITextAutocapitalizationType.None,
				AutocorrectionType = UITextAutocorrectionType.No
			};
			_txtPassword = new UITextField
			{
				Placeholder = "Password",
				BorderStyle = UITextBorderStyle.RoundedRect,
				AutocapitalizationType = UITextAutocapitalizationType.None,
				SecureTextEntry = true
			};
			_btnLogin = new UIButton();
			_btnLogin.SetTitle("Login", UIControlState.Normal);
			_btnLogin.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);

			_btnRegister = new UIButton();
			_btnRegister.SetTitle("Register", UIControlState.Normal);
			_btnRegister.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);

			_btnModal = new UIButton();
			_btnModal.SetTitle("Open modal view", UIControlState.Normal);
			_btnModal.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);
			_viewContainer = new UIView();

			View.AddSubview(_viewContainer);
			_viewContainer.AddSubviews(_txtUsername, _txtPassword, _btnLogin, _btnRegister, _btnModal);
			View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			_viewContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

			View.AddConstraints(
				_viewContainer.WithSameCenterY(View),
				_viewContainer.WithSameLeft(View),
				_viewContainer.WithSameRight(View)
			);
			_viewContainer.AddConstraints(
				_txtUsername.AtTopOf(_viewContainer),
				_txtUsername.WithSameLeft(_viewContainer),
				_txtUsername.WithSameRight(_viewContainer),

				_txtPassword.Below(_txtUsername, 8f),
				_txtPassword.WithSameLeft(_viewContainer),
				_txtPassword.WithSameRight(_viewContainer),

				_btnLogin.Below(_txtPassword, 8f),
				_btnLogin.WithSameLeft(_viewContainer),
				_btnLogin.WithSameRight(_viewContainer),

				_btnRegister.Below(_btnLogin, 8f),
				_btnRegister.WithSameLeft(_viewContainer),
				_btnRegister.WithSameRight(_viewContainer),

				_btnModal.Below(_btnRegister, 16f),
				_btnModal.WithSameLeft(_viewContainer),
				_btnModal.WithSameRight(_viewContainer),
				_btnModal.AtBottomOf(_viewContainer)
			);

			var set = this.CreateBindingSet<LoginView, LoginViewModel>();
			set.Bind(_txtUsername).To(vm => vm.Username);
			set.Bind(_txtPassword).To(vm => vm.Password);
			set.Bind(_btnLogin).To(vm => vm.LoginCommand);
			set.Bind(_btnRegister).To(vm => vm.RegisterCommand);
			set.Bind(_btnModal).To(vm => vm.OpenModalCommand);
			set.Apply();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			NavigationController.SetNavigationBarHidden(true, false);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			NavigationController.SetNavigationBarHidden(false, false);
		}

		private void ClearNavigationStack()
		{
			var vca = NavigationController.ViewControllers;
			foreach(var vc in vca)
			{
				if(!vc.GetType().Equals(this.GetType()))
				{
					vc.RemoveFromParentViewController();
				}
			}
			NavigationItem.HidesBackButton = true;
		}
	}
}

