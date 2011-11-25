using System;
using Cirrious.MonoCross.Extensions.Interfaces;

namespace Cirrious.MonoCross.Extensions.Platform
{
    public class MXServiceFactorySingleton
    {
        public static readonly MXServiceFactorySingleton Instance = new MXServiceFactorySingleton();

        private IMXServiceFactory _serviceFactory;
        public IMXServiceFactory ServiceFactory
        {
            get { return _serviceFactory; }
            set {
                if (_serviceFactory != null)
                {
                    // TODO - commented out for now, but we do need to understand the lifecycle here!
                    //throw new InvalidOperationException("ServiceFactory already set");
                }
                _serviceFactory = value; 
            }
        }
    }
}
