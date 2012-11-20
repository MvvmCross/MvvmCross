#if HACK_DO_NOT_FORWARD_ICOMMAND
// ReSharper disable CheckNamespace
namespace System.Windows.Input
// ReSharper restore CheckNamespace
{
    // removed ICommand as latest monotouch versions have System.Windows.Input in them!
    public interface ICommand
    {
        // Events
        event EventHandler CanExecuteChanged;

        // Methods
        bool CanExecute(object parameter);
        void Execute(object parameter);
    }
}
#endif