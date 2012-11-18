// note that in VisualStudio, MONOTOUCH is not defined
#if false //!MONO_TOUCH
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