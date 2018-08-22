using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavResultDetailViewModel : MvxNavigationViewModelResult<DetailResultResult>, IMvxViewModelResult<DetailResultResult>
    {
        public MixedNavResultDetailViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            CloseViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this, DetailResultResult.Build()));
        }


        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }
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
