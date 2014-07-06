// MvxOpenNetCfIocServiceProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;

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