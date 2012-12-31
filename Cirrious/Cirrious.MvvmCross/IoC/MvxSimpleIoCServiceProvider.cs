#region Copyright

// <copyright file="MvxSimpleIoCServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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