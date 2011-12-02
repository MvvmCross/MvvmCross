#region Copyright

// <copyright file="MvxTouchServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#region using

using Cirrious.MvvmCross.Touch.Services.Tasks;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.Interfaces.Services.Tasks;
using Cirrious.MvvmCross.Platform;

#endregion

namespace Cirrious.MvvmCross.Touch.Services
{
    [MvxServiceProvider]
    public class MvxTouchServiceProvider : MvxPlatformIndependentServiceProvider
    {
        public override void Initialize(IMvxIoCProvider iocProvider)
        {
            base.Initialize(iocProvider);
            SetupPlatformTypes();
        }

        private void SetupPlatformTypes()
        {
            RegisterServiceType<IMvxSimpleFileStoreService, MvxFileStoreService>();
            RegisterServiceType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            RegisterServiceType<IMvxPhoneCallTask, MvxPhoneCallTask>();
        }
    }
}