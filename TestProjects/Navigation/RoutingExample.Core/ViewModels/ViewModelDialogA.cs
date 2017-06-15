using System;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class ViewModelDialogA : MvxViewModel<string, string>
    {
        private readonly IMvxNavigationService _navigationService;
        private int _navigatedAwayFromCount = 0;
        private int _returnedFromCount = 0;
        private string _navigatedAwayFrom;
        private string _returnedFrom;

        public string Title => "View A";

        public string NavigatedAwayFrom
        {
            get => _navigatedAwayFrom;
            set => SetProperty(ref _navigatedAwayFrom, value);
        }

        public string ReturnedFrom
        {
            get => _returnedFrom;
            set => SetProperty(ref _returnedFrom, value);
        }

        public MvxCommand GoToBCommand => new MvxCommand(async () =>
        {
            _navigatedAwayFromCount++;
            NavigatedAwayFrom = $"Navigated to View B {_navigatedAwayFromCount} times";

            var result = await _navigationService.Navigate<ViewModelB, Tuple<string, int>, string>(new Tuple<string, int>(Title, _navigatedAwayFromCount));

            _returnedFromCount++;

            ReturnedFrom = $"Returned from View B {_returnedFromCount} times";
        });

        public ViewModelDialogA(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigatedAwayFrom = $"Navigated to View B {_navigatedAwayFromCount} times";
            ReturnedFrom = $"Returned from View B {_returnedFromCount} times";
        }

        public override Task Initialize(string parameter)
        {
            return Task.FromResult(true);
        }
    }
}
