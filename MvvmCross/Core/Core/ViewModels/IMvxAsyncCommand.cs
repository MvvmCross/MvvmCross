namespace MvvmCross.Core.ViewModels
{
    using System.Threading.Tasks;
    
    public interface IMvxAsyncCommand : IMvxCommand
    {
        Task ExecuteAsync(object parameter = null);
        void Cancel();
    }
}