using System;
using MvvmCross.Core.ViewModels;
namespace MvvmCross.iOS.Support.Tabs.Core.ViewModels
{
	public class RegisterViewModel : MvxViewModel
	{
		public RegisterViewModel()
		{
		}

		private string _simpleText;
		public string SimpleText
		{
			get
			{
				return _simpleText;
			}
			set
			{
				_simpleText = value;
				RaisePropertyChanged(() => SimpleText);
			}
		}

		public override void Start()
		{
			base.Start();

			SimpleText = "Welcome!";
		}
	}
}

