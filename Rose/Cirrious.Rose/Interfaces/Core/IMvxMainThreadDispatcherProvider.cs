namespace Cirrious.CrossCore.Interfaces.Core
{
    public interface IMvxMainThreadDispatcherProvider
    {
        IMvxMainThreadDispatcher Dispatcher { get; }
    }
}