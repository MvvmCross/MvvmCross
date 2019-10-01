using System.Threading.Tasks;
using Example.Core.Model;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Example.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public LoginViewModel(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;

            Username = "TestUser";
            Password = "YouCantSeeMe";
            IsLoading = false;
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public virtual IMvxCommand LoginCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsLoading = !IsLoading; //Toggle for testing
                    ShowViewModel<HomeViewModel>();
                });
            }
        }

        public virtual IMvxAsyncCommand ShowDialogCommand
        {
            get
            {
                return new MvxAsyncCommand(ExecuteShowDialogCommandAsync);
            }
        }

        private async Task ExecuteShowDialogCommandAsync()
        {
            var confirmationResult = await _mvxNavigationService.Navigate<ConfirmationViewModel, ConfirmationConfiguration, bool?>(
                 new ConfirmationConfiguration
                 {
                     Body = "Confirm this message",
                     Title = "Example App",
                     PositiveCommandText = "Yes",
                     NegativeCommandText = "No"
                 });

            Mvx.Trace($"ConfirmationViewModel navigation returned {confirmationResult}.");
        }
    }
}