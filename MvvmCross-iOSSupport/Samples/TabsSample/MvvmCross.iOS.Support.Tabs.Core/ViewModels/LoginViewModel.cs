using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using Plugin.Settings;
using MvvmCross.iOS.Support.Tabs.Core.Helpers;

namespace MvvmCross.iOS.Support.Tabs.Core.ViewModels
{
	public class LoginViewModel : MvxViewModel
	{
		public LoginViewModel()
		{
			LoginCommand = new MvxAsyncCommand(LoginAsync);
			RegisterCommand = new MvxCommand(() => ShowViewModel<RegisterViewModel>());
			OpenModalCommand = new MvxCommand(() => ShowViewModel<ModalViewModel>());
		}

		private string _username;
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
				RaisePropertyChanged(() => Username);
			}
		}

		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				RaisePropertyChanged(() => Password);
			}
		}

		public ICommand LoginCommand { get; private set; }

		public ICommand RegisterCommand { get; private set; }

		public ICommand OpenModalCommand { get; private set; }

		private async Task LoginAsync()
		{
			await Task.Delay(100);

			if(string.IsNullOrEmpty(Username))
				return;

			CrossSettings.Current.AddOrUpdateValue<string>(Settings.TokenKey, Username);

			ShowViewModel<MainViewModel>();
		}
	}
}

