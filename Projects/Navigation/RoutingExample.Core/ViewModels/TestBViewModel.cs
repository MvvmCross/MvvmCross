using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoutingExample.Core.ViewModels;

[assembly: MvxNavigation(typeof(TestBViewModel), @"mvx://test/\?id=(?<id>[A-Z0-9]{32})$")]
namespace RoutingExample.Core.ViewModels
{
    public class TestBViewModel
        : MvxViewModel<User, User>
    {
        private readonly IMvxNavigationService _navigationService;
        public TestBViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public void Init()
        {
            _user = new User($"Initial view {GetHashCode()}", "Test");
        }

        private User _user;

        public IMvxAsyncCommand CloseViewModelCommand => new MvxAsyncCommand(
            () => _navigationService.Close(this, new User("Return result", "Something")));
        public IMvxAsyncCommand OpenViewModelMainCommand => new MvxAsyncCommand(
            () => _navigationService.Navigate<MainViewModel>());
        public IMvxAsyncCommand OpenViewModelACommand => new MvxAsyncCommand(
            () =>  _navigationService.Navigate<TestAViewModel, User>(new User($"To A from {GetHashCode()}", "Something")));
        public IMvxAsyncCommand OpenViewModelBCommand => new MvxAsyncCommand(
            () =>  _navigationService.Navigate<TestBViewModel, User, User>(new User($"To B from {GetHashCode()}", "Something")));

        public override void Prepare(User parameter)
        {
            _user = parameter;
        }
    }
}
