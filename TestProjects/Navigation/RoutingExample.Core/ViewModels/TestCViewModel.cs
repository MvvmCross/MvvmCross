using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class TestCViewModel
        : MvxViewModel<User, User>
    {
        private readonly IMvxNavigationService _navigationService;
        public TestCViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private string _parameter;
        public string Parameter
        {
            get { return _parameter; }
            set { SetProperty(ref _parameter, value); }
        }

        public void Init(string viewmodelcparameter)
        {
            Parameter = viewmodelcparameter;
            _user = new User($"Initial view {GetHashCode()}", "Test");
        }

        private User _user;

        public IMvxAsyncCommand CloseViewModelCommand => new MvxAsyncCommand(
            () => _navigationService.Close(this, new User("Return result", "Something")));

        public override void Prepare(User parameter)
        {
            _user = parameter;
        }
    }
}
