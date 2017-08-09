using System;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class TestCViewModel
        : MvxViewModel<int, int>
    {
        private readonly IMvxNavigationService _navigationService;
        public TestCViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        private int _userId;
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        public IMvxAsyncCommand CloseViewModelCommand => new MvxAsyncCommand(
                () => _navigationService.Close(this, UserId));

        public override void Prepare(int parameter)
        {
            UserId = parameter;
        }
    }
}