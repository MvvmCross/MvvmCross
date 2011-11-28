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