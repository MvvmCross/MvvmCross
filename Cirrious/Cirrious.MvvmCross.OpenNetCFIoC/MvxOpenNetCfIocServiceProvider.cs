// MvxOpenNetCfIocServiceProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;

#endregion

namespace Cirrious.MvvmCross.OpenNetCfIoC
{
    public class MvxOpenNetCfIocServiceProvider : IMvxIoCProvider
    {
        public bool CanResolve<T>() where T : class
        {
            return MvxOpenNetCfContainer.Current.CanResolve<T>();
        }

        public T Resolve<T>() where T : class
        {
            return MvxOpenNetCfContainer.Current.Resolve<T>();
        }

        public bool TryResolve<T>(out T service) where T : class
        {
            return MvxOpenNetCfContainer.Current.TryResolve(out service);
        }

        public void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom
        {
            MvxOpenNetCfContainer.Current.RegisterServiceType<TFrom, TTo>();
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            MvxOpenNetCfContainer.Current.RegisterServiceInstance(theObject);
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            MvxTrace.Trace(MvxTraceLevel.Warning,
                           "Lazy constructor not implemented for OpenNetCF IoC - so constructing now");
            var theObject = theConstructor();
            MvxOpenNetCfContainer.Current.RegisterServiceInstance(theObject);
        }
    }
}