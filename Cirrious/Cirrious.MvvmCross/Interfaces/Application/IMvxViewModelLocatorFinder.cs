using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Interfaces.Application
{
    public interface IMvxViewModelLocatorFinder
    {
        IMvxViewModelLocator FindLocator(MvxShowViewModelRequest request);
    }
}