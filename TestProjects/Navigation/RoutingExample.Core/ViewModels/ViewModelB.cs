using System;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class ViewModelB : MvxViewModel<Tuple<string,int>, string>
    {
        private readonly IMvxNavigationService _navigationService;
        private int _navigatedAwayFromCount = 0;
        private int _returnedFromCount = 0;
        private string _navigatedAwayFrom;
        private string _navigatedTo;
        private string _returnedFrom;

        public string Title => "View B";

        public string NavigatedAwayFrom
        {
            get => _navigatedAwayFrom;
            set => SetProperty(ref _navigatedAwayFrom, value);
        }

        public string NavigatedTo
        {
            get => _navigatedTo;
            set => SetProperty(ref _navigatedTo, value);
        }

        public string ReturnedFrom
        {
            get => _returnedFrom;
            set => SetProperty(ref _returnedFrom, value);
        }

        public MvxCommand GoToCCommand => new MvxCommand(async () =>
        {
            _navigatedAwayFromCount++;

            NavigatedAwayFrom = $"Navigated to View C {_navigatedAwayFromCount} times";

            var result = await _navigationService.Navigate<ViewModelC, Tuple<string, int>, string>(new Tuple<string, int>(Title, _navigatedAwayFromCount));

            _returnedFromCount++;

            ReturnedFrom = $"Returned from View C {_returnedFromCount} times";
        });

        public MvxCommand CloseCommand => new MvxCommand(async () =>
        {
            await _navigationService.Close(this, Title);
        });

        public ViewModelB(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigatedAwayFrom = $"Navigated to View C {_navigatedAwayFromCount} times";

            ReturnedFrom = $"Returned from View C {_returnedFromCount} times";
        }

        public override void Prepare(Tuple<string, int> parameter)
        {
            NavigatedTo = $"Navigated to from {parameter.Item1} {parameter.Item2} times";
        }
    }
}
