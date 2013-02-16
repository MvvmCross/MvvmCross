namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxMainThreadDispatcherProvider
    {
        IMvxMainThreadDispatcher Dispatcher { get; }
    }
}