// IMvxViewModelLocatorStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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