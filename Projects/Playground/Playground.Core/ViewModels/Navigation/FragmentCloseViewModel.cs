using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace Playground.Core.ViewModels.Navigation
{
    public class FragmentCloseViewModel : BaseViewModel
    {
        private static int _counter = 0;

        public FragmentCloseViewModel(ILoggerFactory loggerFactory, IMvxNavigationService navigationService)
            : base(loggerFactory, navigationService)
        {
            ForwardCommand = new MvxAsyncCommand(() => NavigationService.Navigate<FragmentCloseViewModel>());
            CloseCommand = new MvxAsyncCommand(() => NavigationService.Close(this));

            Description = $"View number {_counter++}";
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public IMvxAsyncCommand ForwardCommand { get; }
        public IMvxAsyncCommand CloseCommand { get; }
    }
}
