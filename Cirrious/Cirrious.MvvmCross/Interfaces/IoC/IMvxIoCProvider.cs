namespace Cirrious.MvvmCross.Interfaces.IoC
{
    public interface IMvxIoCProvider
    {
        T GetService<T>() where T : class;
        void RegisterServiceType<TFrom, TTo>();
        void RegisterServiceInstance<TInterface>(TInterface theObject);
    }
}