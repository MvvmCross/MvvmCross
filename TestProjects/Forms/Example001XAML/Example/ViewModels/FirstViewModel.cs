using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Example.ViewModels
{
    public class FirstViewModel
        : MvxViewModel
    {
        private string _yourNickname = string.Empty;
        public string YourNickname
        {
            get
            {
                return _yourNickname;
            }
            set
            {
                SetProperty(ref _yourNickname, value);
                RaisePropertyChanged(() => Hello);
            }
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
