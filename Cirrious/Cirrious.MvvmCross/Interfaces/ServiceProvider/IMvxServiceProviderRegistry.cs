namespace Cirrious.MvvmCross.Interfaces.ServiceProvider
{
    public interface IMvxServiceProviderRegistry : IMvxServiceProvider
    {
        void RegisterServiceType<TFrom, TTo>();
        void RegisterServiceInstance<TInterface>(TInterface theObject);
    }
}