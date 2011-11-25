namespace Cirrious.MonoCross.Extensions.Platform
{
    public static class MXServiceFactoryExtensions
    {
        public static TService GetService<TService>(this IMXServiceConsumer consumer) where TService : class
        {
            var factory = MXServiceFactorySingleton.Instance.ServiceFactory;

            if (factory == null)
                return default(TService);

            return factory.CreateService<TService>();
        }
    }
}