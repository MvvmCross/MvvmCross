using System;
using MvvmCross.Core.ViewModels;
using Plugin.Settings;
using MvvmCross.iOS.Support.Tabs.Core.Helpers;
using MvvmCross.iOS.Support.Tabs.Core.ViewModels;

namespace MvvmCross.iOS.Support.Tabs.Core
{
	public class AppStart : MvxNavigatingObject, IMvxAppStart
	{
		public void Start(object hint = null)
		{
			if(!string.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault<string>(Settings.TokenKey)))
			{
				ShowViewModel<MainViewModel>();
			}
			else
			{
				ShowViewModel<LoginViewModel>();
			}
		}
	}
}

