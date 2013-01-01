// MvxSimpleIoCServiceProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.IoC;

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
            return MvxSimpleIoCContainer.Instance.TryResolve(out service);
        }

        public void RegisterServiceType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom
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