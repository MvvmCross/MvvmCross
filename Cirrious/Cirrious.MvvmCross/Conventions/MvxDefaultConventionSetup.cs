#region Copyright
// <copyright file="MvxDefaultConventionSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Conventions
{
    public sealed class MvxDefaultConventionSetup
        : IMvxServiceProducer<IMvxViewModelLocatorAnalyser>
    {
        private MvxDefaultConventionSetup()
        {
        }

        public static void Initialize()
        {
            new MvxDefaultConventionSetup().InitializeImpl();
        }

        private void InitializeImpl()
        {
            this.RegisterServiceType<IMvxViewModelLocatorAnalyser, MvxViewModelLocatorAnalyser>();
        }
    }
}