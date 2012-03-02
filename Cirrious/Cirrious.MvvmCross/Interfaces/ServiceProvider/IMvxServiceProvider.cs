#region Copyright
// <copyright file="IMvxServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
namespace Cirrious.MvvmCross.Interfaces.ServiceProvider
{
    public interface IMvxServiceProvider
    {
        bool SupportsService<T>() where T : class;
        T GetService<T>() where T : class;
        bool TryGetService<T>(out T service) where T : class;
    }
}