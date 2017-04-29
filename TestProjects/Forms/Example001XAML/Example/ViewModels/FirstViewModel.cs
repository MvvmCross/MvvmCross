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
            get => _yourNickname;
            set
            {
                _yourNickname = value;
                RaisePropertyChanged(() => YourNickname);
                RaisePropertyChanged(() => Hello);
            }
        }

        public string Hello => "Hello " + YourNickname;


        public ICommand ShowAboutPageCommand
        {
            get { return new MvxCommand(() => ShowViewModel<AboutViewModel>()); }
        }
    }
}