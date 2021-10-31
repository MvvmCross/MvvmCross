using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavResultDetailViewModel : MvxNavigationViewModel
    {
        public MixedNavResultDetailViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseViewModelCommand = new MvxAsyncCommand(() => NavigationService.Close(this));
        }

        public IMvxAsyncCommand CloseViewModelCommand { get; }
    }
}
