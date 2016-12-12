using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using Plugin.Settings;
using MvvmCross.iOS.Support.Tabs.Core.Helpers;

namespace MvvmCross.iOS.Support.Tabs.Core.ViewModels
{
	public class Tab2ViewModel : MvxViewModel
	{
		public Tab2ViewModel()
		{
			LogoutCommand = new MvxAsyncCommand(LogoutAsync);
		}

		public ICommand LogoutCommand { get; private set; }

		private async Task LogoutAsync()
		{
			await Task.Delay(100);

			CrossSettings.Current.AddOrUpdateValue<string>(Settings.TokenKey, string.Empty);

			ShowViewModel<LoginViewModel>();
		}
	}
}

