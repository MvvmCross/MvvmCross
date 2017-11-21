using System;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Test.Mocks.ViewModels
{
    public class SimpleResultTestViewModel : MvxViewModelResult<bool>
    {
        public SimpleResultTestViewModel()
        {
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await Task.Delay(2000);
            CloseCompletionSource.SetResult(true);
        }
    }
}
