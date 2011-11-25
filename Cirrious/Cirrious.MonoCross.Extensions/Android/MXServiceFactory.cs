using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Platform;

namespace Cirrious.MonoCross.Extensions.Android.Android
{
    [MXServiceFactory]
    public class MXServiceFactory : IMXServiceFactory
    {
        public T CreateService<T>() where T : class
        {
            var targetType = typeof (T);

            if (targetType == typeof(IMXSimpleFileStoreService))
            {
                return new MXFileStoreService() as T;
            }

            return default(T);
        }
    }
}