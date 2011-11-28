#region Copyright
// <copyright file="MvxPlatfromIndependentServiceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.ViewModel;

namespace Cirrious.MvvmCross.Platform
{
    public class MvxPlatfromIndependentServiceProvider : MvxServiceProvider
    {
        public override void Initialize(IMvxIoCProvider iocProvider)
        {
            base.Initialize(iocProvider);
            SetupPlatfromIndependentServices();
        }

        private void SetupPlatfromIndependentServices()
        {
            RegisterServiceType<IMvxViewModelLoader, MvxViewModelLoader>();
        }
    }
}