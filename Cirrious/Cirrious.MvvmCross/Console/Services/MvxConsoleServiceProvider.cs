#region Copyright
// <copyright file="MvxConsoleServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using Cirrious.MvvmCross.Console.Services.Tasks;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;
using Cirrious.MvvmCross.Platform;

#endregion

namespace Cirrious.MvvmCross.Console.Services
{
    [MvxServiceProvider]
    public class MvxConsoleServiceProvider : MvxPlatformIndependentServiceProvider
    {
        public override void Initialize(IMvxIoCProvider iocProvider)
        {
            base.Initialize(iocProvider);
            SetupPlatformTypes();
        }

        private void SetupPlatformTypes()
        {
            RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            RegisterServiceInstance<IMvxResourceLoader>(new MvxConsoleResourceLoader());
            RegisterServiceType<IMvxSimpleFileStoreService, MvxFileStoreService>();
            RegisterServiceType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            RegisterServiceType<IMvxPhoneCallTask, MvxPhoneCallTask>();
        }
    }
}