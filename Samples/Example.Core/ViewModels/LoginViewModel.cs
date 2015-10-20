using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Example.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        public LoginViewModel () {
            this.Username = "TestUser";
            this.Password = "YouCantSeeMe";
            this.IsLoading = false;
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty (ref _username, value);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty (ref _password, value);
            }
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        public virtual IMvxCommand LoginCommand
        {
            get
            {
                return new MvxCommand( () => ShowViewModel<MainViewModel>() );
            }
        }
    }
}

