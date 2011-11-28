using Cirrious.MvvmCross.Interfaces.IoC;

namespace Cirrious.MvvmCross.Interfaces.ServiceProvider
{
    public interface IMvxServiceProviderSetup : IMvxServiceProviderRegistry
    {
        void Initialize(IMvxIoCProvider iocProvider);
    }
}