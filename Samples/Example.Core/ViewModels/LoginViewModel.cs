using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Example.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        public LoginViewModel () {
            this.Username = "TestUser";
            this.Password = "YouCantSeeMe";
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public virtual ICommand Login
        {
            get
            {
                return new MvxCommand( () => ShowViewModel<MainViewModel>() );
            }
        }
    }
}

