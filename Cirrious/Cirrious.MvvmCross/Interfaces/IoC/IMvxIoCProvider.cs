// IMvxIoCProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region Copyright

// <copyright file="IMvxIoCProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
using System;

#endregion

namespace Cirrious.MvvmCross.Interfaces.IoC
{
    public interface IMvxIoCProvider
    {
        bool SupportsService<T>()
            where T : class;

        T GetService<T>()
            where T : class;

        bool TryGetService<T>(out T service)
            where T : class;

        void RegisterServiceType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterServiceInstance<TInterface>(TInterface theObject)
            where TInterface : class;

        void RegisterServiceInstance<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class;
    }
}