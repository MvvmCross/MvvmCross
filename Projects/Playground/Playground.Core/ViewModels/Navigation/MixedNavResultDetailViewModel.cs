using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavResultDetailViewModel : MvxNavigationViewModelResult<DetailResultResult>
    {
        public MixedNavResultDetailViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseViewModelCommand = new MvxAsyncCommand(() => NavigationService.Close(this, DetailResultResult.Build()));
        }

        public IMvxAsyncCommand CloseViewModelCommand { get; }
    }

    public class DetailResultParams
    {
    }

    public class DetailResultResult
    {
        public static DetailResultResult Build()
        {
            return new DetailResultResult();
        }
    }
}
