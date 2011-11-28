using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.IoC;

namespace Cirrious.MvvmCross.IoC
{
    public class MvxOpenNetCfIocProvider : IMvxIoCProvider
    {
        public virtual T GetService<T>() where T : class
        {
            return IoC.MvxOpenNetCfContainer.Current.Resolve<T>();
        }

        public virtual void RegisterServiceType<TFrom, TTo>()
        {
            IoC.MvxOpenNetCfContainer.Current.RegisterServiceType<TFrom, TTo>();
        }

        public virtual void RegisterServiceInstance<TInterface>(TInterface theObject)
        {
            IoC.MvxOpenNetCfContainer.Current.RegisterServiceInstance(theObject);
        }
    }
}