#region Copyright
// <copyright file="MvxOpenNetCfServiceProviderSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.IoC
{
    public static class MvxOpenNetCfServiceProviderSetup
    {
        public static void Initialize()
        {
            var ioc = new MvxOpenNetCfIocProvider();
            MvxServiceProviderSetup.Initialize(ioc);
        }

        public static void Initialize(Type serviceProviderType)
        {
            var ioc = new MvxOpenNetCfIocProvider();
            MvxServiceProviderSetup.Initialize(serviceProviderType, ioc);
        }
    }
}