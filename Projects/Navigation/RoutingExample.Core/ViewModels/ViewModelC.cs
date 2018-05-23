using System;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class ViewModelC : MvxViewModel<Tuple<string, int>, string>
    {
        private readonly IMvxNavigationService _navigationService;
        private string _navigatedTo;

        public ViewModelC(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public string Title => "View C";

        public string NavigatedTo
        {
            get => _navigatedTo;
            set => SetProperty(ref _navigatedTo, value);
        }

        public MvxCommand CloseCommand => new MvxCommand(async () =>
        {
            await _navigationService.Close(this, Title);
        });

        public override void Prepare(Tuple<string, int> parameter)
        {
            NavigatedTo = $"Navigated to from {parameter.Item1} {parameter.Item2} times";
        }
    }
}
