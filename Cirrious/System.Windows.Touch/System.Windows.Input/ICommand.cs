// ReSharper disable CheckNamespace
namespace System.Windows.Input
// ReSharper restore CheckNamespace
{
    public interface ICommand2
    {
        // Events
        event EventHandler CanExecuteChanged;

        // Methods
        bool CanExecute(object parameter);
        void Execute(object parameter);
    }
}