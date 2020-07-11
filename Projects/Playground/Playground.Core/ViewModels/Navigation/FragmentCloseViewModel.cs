using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace Playground.Core.ViewModels.Navigation
{
    public class FragmentCloseViewModel : BaseViewModel
    {
        private static int _counter = 0;

        public FragmentCloseViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ForwardCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<FragmentCloseViewModel>().ConfigureAwait(false));
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(false));

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
