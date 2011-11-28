using System.Windows.Input;

namespace Cirrious.MvvmCross.Interfaces.Commands
{
#if WINDOWS_PHONE
    public interface IMvxCommand : ICommand
    {
        
    }
#else
    public interface IMvxCommand
    {
        bool CanExecute(object parameter);
        void Execute(object parameter);
        event EventHandler CanExecuteChanged;
    }
#endif
}