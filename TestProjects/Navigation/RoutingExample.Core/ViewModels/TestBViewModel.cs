using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public TestBViewModel()
        {

        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public void Init()
        {
            _user = new User($"Initial view {this.GetHashCode()}", "Test");
        }

        private User _user;

        public IMvxAsyncCommand CloseViewModelCommand => new MvxAsyncCommand(
            () => Close(new User("Return result", "Something")));
        public IMvxAsyncCommand OpenViewModelMainCommand => new MvxAsyncCommand(
            () => Mvx.Resolve<IMvxNavigationService>().Navigate<MainViewModel>());
        public IMvxAsyncCommand OpenViewModelACommand => new MvxAsyncCommand(
            () =>  Mvx.Resolve<IMvxNavigationService>().Navigate<TestAViewModel, User>(new User($"To A from {this.GetHashCode()}", "Something")));
        public IMvxAsyncCommand OpenViewModelBCommand => new MvxAsyncCommand(
            () =>  Mvx.Resolve<IMvxNavigationService>().Navigate<TestBViewModel, User, User>(new User($"To B from {this.GetHashCode()}", "Something")));

        public override async Task Initialize(User parameter)
        {
            _user = parameter;
        }
    }
}
