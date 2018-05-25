using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavResultDetailViewModel : MvxViewModel, IMvxViewModelResult<DetailResultResult>
    {
        public MixedNavResultDetailViewModel()
        {
            CloseViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this, DetailResultResult.Build()));
        }


        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }
        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            base.ViewDestroy(viewFinishing);
        }
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
