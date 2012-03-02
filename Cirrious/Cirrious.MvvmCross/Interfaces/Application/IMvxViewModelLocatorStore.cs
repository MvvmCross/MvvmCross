#region Copyright
// <copyright file="IMvxViewModelLocatorStore.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Interfaces.Application
{
    public interface IMvxViewModelLocatorStore
    {
        void AddLocators(IEnumerable<IMvxViewModelLocator> locators);
        void AddLocator(IMvxViewModelLocator locator);
    }
}