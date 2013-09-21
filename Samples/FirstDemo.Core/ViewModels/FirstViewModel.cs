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

		public string FullName
		{
			get { return string.Format ("{0} {1}", _firstName, _lastName); }
		}
	}
}