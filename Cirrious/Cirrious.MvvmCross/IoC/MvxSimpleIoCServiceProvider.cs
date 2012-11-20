using Cirrious.MvvmCross.Interfaces.IoC;
using System;

namespace Cirrious.MvvmCross.IoC
{
    public class MvxSimpleIoCServiceProvider : IMvxIoCProvider
    {
        #region IMvxIoCProvider Members

        public bool SupportsService<T>() where T : class
        {
            return MvxSimpleIoCContainer.Current.CanResolve<T>();
        }

        public T GetService<T>() where T : class
        {
            return MvxSimpleIoCContainer.Current.Resolve<T>();
        }

        public bool TryGetService<T>(out T service) where T : class
        {
            return MvxSimpleIoCContainer.Current.TryResolve<T>(out service);
        }

        public void RegisterServiceType<TFrom, TTo>() 
            where TFrom : class 
            where TTo : class 
        {
            MvxSimpleIoCContainer.Current.RegisterServiceType<TFrom, TTo>();
        }

        public void RegisterServiceInstance<TInterface>(TInterface theObject)
            where TInterface : class
        {
            MvxSimpleIoCContainer.Current.RegisterServiceInstance(theObject);
        }

		public void RegisterServiceInstance<TInterface>(Func<TInterface> theConstructor)
			where TInterface : class
		{
			MvxSimpleIoCContainer.Current.RegisterServiceInstance(theConstructor);
		}

        #endregion
    }
}