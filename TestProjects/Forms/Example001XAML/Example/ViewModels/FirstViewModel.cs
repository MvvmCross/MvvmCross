using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace Example.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
		private string _yourNickname = string.Empty;
        public string YourNickname
		{ 
			get { return _yourNickname; }
			set { _yourNickname = value; RaisePropertyChanged(() => YourNickname); RaisePropertyChanged(() => Hello); }
		}

        public string Hello
        {
            get { return "Hello " + YourNickname; }
        }


        public ICommand ShowAboutPageCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<AboutViewModel>());
            }
        }
    }
}
