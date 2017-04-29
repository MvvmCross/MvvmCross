using MvvmCross.Core.ViewModels;

namespace Example.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private bool _isLoading;

        private string _password;

        private string _username;

        public LoginViewModel()
        {
            Username = "TestUser";
            Password = "YouCantSeeMe";
            IsLoading = false;
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public virtual IMvxCommand LoginCommand
        {
            get { return new MvxCommand(() => ShowViewModel<HomeViewModel>()); }
        }
    }
}