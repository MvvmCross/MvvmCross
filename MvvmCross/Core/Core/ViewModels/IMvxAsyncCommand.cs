using System.Threading.Tasks;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxAsyncCommand : IMvxCommand
    {
        Task ExecuteAsync(object parameter = null);
        void Cancel();
    }
}