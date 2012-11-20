#region Copyright
// <copyright file="MvxOpenNetCfIocServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using System;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;


#endregion

using Cirrious.MvvmCross.Interfaces.IoC;

namespace Cirrious.MvvmCross.OpenNetCfIoC
{
    public class MvxOpenNetCfIocServiceProvider : IMvxIoCProvider
    {
        #region IMvxIoCProvider Members

        public bool SupportsService<T>() where T : class
        {
            return MvxOpenNetCfContainer.Current.CanResolve<T>();
        }

        public T GetService<T>() where T : class
        {
            return MvxOpenNetCfContainer.Current.Resolve<T>();
        }

        public bool TryGetService<T>(out T service) where T : class
        {
            return MvxOpenNetCfContainer.Current.TryResolve<T>(out service);
        }

        public void RegisterServiceType<TFrom, TTo>()
            where TFrom : class
            where TTo : class
        {
            MvxOpenNetCfContainer.Current.RegisterServiceType<TFrom, TTo>();
        }

        public void RegisterServiceInstance<TInterface>(TInterface theObject)
            where TInterface : class
        {
            MvxOpenNetCfContainer.Current.RegisterServiceInstance(theObject);
        }

		public void RegisterServiceInstance<TInterface>(Func<TInterface> theConstructor)
			where TInterface : class
		{
			MvxTrace.Trace(MvxTraceLevel.Warning, "Lazy constructor not implemented for OpenNetCF IoC - so constructing now");
			var theObject = theConstructor();
			MvxOpenNetCfContainer.Current.RegisterServiceInstance(theObject);
		}

        #endregion
    }
}