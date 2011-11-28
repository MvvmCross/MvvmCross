using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModel;

namespace Cirrious.MvvmCross.Interfaces.Application
{
    public interface IMvxViewModelLocatorStore
    {
        void AddLocators(IEnumerable<IMvxViewModelLocator> locators);
        void AddLocator(IMvxViewModelLocator locator);
    }
}