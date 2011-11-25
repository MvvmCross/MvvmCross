using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Platform;
using System.Collections.Generic;
using System;

namespace Cirrious.MonoCross.Extensions.WindowsPhone
{
    [MXServiceFactory]
    public class MXServiceFactory : IMXServiceFactory
    {
        private readonly Dictionary<Type, object> _registeredInstances = new Dictionary<Type, object>();
        
        public T CreateService<T>() where T : class
        {
            var targetType = typeof (T);

            if (targetType == typeof(IMXSimpleFileStoreService))
            {
                return new MXIsolatedStorageFileStoreService() as T;
            }

            return default(T);
        }
    }
}