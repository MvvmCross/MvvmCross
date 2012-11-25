using Cirrious.MvvmCross.Interfaces.IoC;
using System;

namespace Cirrious.MvvmCross.IoC
{
    public class MvxSimpleIoCServiceProvider : IMvxIoCProvider
    {
        public MvxSimpleIoCServiceProvider()
        {
            MvxSimpleIoCContainer.Initialise();
        }

        #region IMvxIoCProvider Members

        public bool SupportsService<T>() where T : class
        {
            return MvxSimpleIoCContainer.Instance.CanResolve<T>();
        }

        public T GetService<T>() where T : class
        {
            return MvxSimpleIoCContainer.Instance.Resolve<T>();
        }

        public bool TryGetService<T>(out T service) where T : class
        {
            return MvxSimpleIoCContainer.Instance.TryResolve<T>(out service);
        }

        public void RegisterServiceType<TFrom, TTo>() 
            where TFrom : class 
            where TTo : class 
        {
            MvxSimpleIoCContainer.Instance.RegisterServiceType<TFrom, TTo>();
        }

        public void RegisterServiceInstance<TInterface>(TInterface theObject)
            where TInterface : class
        {
            MvxSimpleIoCContainer.Instance.RegisterServiceInstance(theObject);
        }

		public void RegisterServiceInstance<TInterface>(Func<TInterface> theConstructor)
			where TInterface : class
		{
            MvxSimpleIoCContainer.Instance.RegisterServiceInstance(theConstructor);
		}

        #endregion
    }
}