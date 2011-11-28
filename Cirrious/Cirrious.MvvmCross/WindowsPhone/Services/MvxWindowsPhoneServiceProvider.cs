#region Copyright
// <copyright file="MvxWindowsPhoneServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.Platform;

#endregion

namespace Cirrious.MvvmCross.WindowsPhone.Services
{
    [MvxServiceProvider]
    public class MvxWindowsPhoneServiceProvider : MvxServiceProvider
    {
        public MvxWindowsPhoneServiceProvider()
        {
        }

        public override void Initialize(IMvxIoCProvider iocProvider)
        {
            base.Initialize(iocProvider);
            SetupPlatformTypes();
        }

        private void SetupPlatformTypes()
        {
            RegisterServiceType<IMvxSimpleFileStoreService, MvxIsolatedStorageFileStoreService>();
            RegisterServiceType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            RegisterServiceType<IMvxPhoneCallTask, MvxPhoneCallTask>();
        }
    }
}