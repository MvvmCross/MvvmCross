using Cirrious.MvvmCross.ViewModels;

namespace FirstDemo.Core.ViewModels
{
	public class FirstViewModel : MvxViewModel
	{
		public FirstViewModel()
		{
		}

		private string _firstName = string.Empty;
		public string FirstName
		{
			get { return _firstName; } 
			set { _firstName = value;
				RaisePropertyChanged (() => FirstName);
				RaisePropertyChanged (() => FullName);
			}
		}

		private string _lastName = string.Empty;
		public string LastName
		{
			get { return _lastName; } 
			set { _lastName = value;
				RaisePropertyChanged (() => LastName); 
				RaisePropertyChanged (() => FullName);
			}
		}

		public IMvxCommand GoCommand
		{
			get 
			{
				return new MvxCommand(() => {
					ShowViewModel<SecondViewModel>();
				});
			}
		}

		public string FullName
		{
			get { return string.Format ("{0} {1}", _firstName, _lastName); }
		}
	}
	public class SecondViewModel : MvxViewModel
	{
		public SecondViewModel()
		{
		}

		private string _firstName = string.Empty;
		public string FirstName
		{
			get { return _firstName; } 
			set { _firstName = value;
				RaisePropertyChanged (() => FirstName);
				RaisePropertyChanged (() => FullName);
			}
		}

		private string _lastName = string.Empty;
		public string LastName
		{
			get { return _lastName; } 
			set { _lastName = value;
				RaisePropertyChanged (() => LastName); 
				RaisePropertyChanged (() => FullName);
			}
		}

		private int _sl = 15;
		public int SliderValue
		{
			get { return _sl; } 
			set { _sl = value;
				RaisePropertyChanged (() => SliderValue);
			}
		}

		private bool _onOffValue;
		public bool OnOffValue {
			get { return _onOffValue; }
			set { _onOffValue = value;
				RaisePropertyChanged (() => OnOffValue); }
		}

		public string FullName
		{
			get { return string.Format ("{0} {1}", _firstName, _lastName); }
		}
	}
}