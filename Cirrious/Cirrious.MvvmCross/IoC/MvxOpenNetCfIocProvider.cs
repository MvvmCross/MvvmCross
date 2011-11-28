#region Copyright
// <copyright file="MvxOpenNetCfIocProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.IoC;

namespace Cirrious.MvvmCross.IoC
{
    public class MvxOpenNetCfIocProvider : IMvxIoCProvider
    {
        #region IMvxIoCProvider Members

        public virtual T GetService<T>() where T : class
        {
            return MvxOpenNetCfContainer.Current.Resolve<T>();
        }

        public virtual void RegisterServiceType<TFrom, TTo>()
        {
            MvxOpenNetCfContainer.Current.RegisterServiceType<TFrom, TTo>();
        }

        public virtual void RegisterServiceInstance<TInterface>(TInterface theObject)
        {
            MvxOpenNetCfContainer.Current.RegisterServiceInstance(theObject);
        }

        #endregion
    }
}