using System.Threading.Tasks;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxAsyncCommand : IMvxCommand
    {
        Task ExecuteAsync(object parameter = null);
        void Cancel();
    }

    public interface IMvxAsyncCommand<T> : IMvxCommand<T>
    {
        Task ExecuteAsync(T parameter);
        void Cancel();
    }
}