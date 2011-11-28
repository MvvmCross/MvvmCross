using System.Linq;
using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Platform
{
    public class MvxServiceProvider : MvxSingleton<IMvxServiceProviderSetup>, IMvxServiceProviderSetup
    {
        private IMvxIoCProvider _ioc;

        public virtual void Initialize(IMvxIoCProvider iocProvider)
        {
            if (_ioc != null)
                throw new MvxException("IoC provider already set");

            _ioc = iocProvider;
        }

        public virtual T GetService<T>() where T : class
        {
#if DEBUG
            if (_ioc == null)
                throw new MvxException("IoC provider not set");
#endif
            return _ioc.GetService<T>();
        }

        public virtual void RegisterServiceType<TFrom, TTo>()
        {
#if DEBUG
            if (_ioc == null)
                throw new MvxException("IoC provider not set");
#endif
            _ioc.RegisterServiceType<TFrom, TTo>();
        }

        public virtual void RegisterServiceInstance<TInterface>(TInterface theObject)
        {
#if DEBUG
            if (_ioc == null)
                throw new MvxException("IoC provider not set");
#endif
            _ioc.RegisterServiceInstance(theObject);
        }
    }
}