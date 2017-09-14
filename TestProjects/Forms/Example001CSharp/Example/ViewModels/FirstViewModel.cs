using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;

namespace Example.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
		private string _yourNickname = "???";
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

        public override async Task Initialize()
        {
            await Task.Delay(1);
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
